using LandmarkEmulator.Shared.Network.Cryptography;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.Shared.Network.Message.Model;
using LandmarkEmulator.Shared.Network.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LandmarkEmulator.Shared.Network
{
    public abstract class GameSession : NetworkSession
    {
        /// <summary>
        /// Determines if queued incoming packets can be processed during a world update.
        /// </summary>
        public bool CanProcessPackets { get; set; } = true;
        public ProtocolVersion ProtocolVersion { get; private set; }
        public string EncryptionKey { get; protected set; }

        private readonly ConcurrentQueue<ProtocolPacket> incomingPackets = new();
        private readonly Queue<ProtocolPacket> outgoingPackets = new();
        private readonly DataStreamInput inputStream;
        private readonly DataStreamOutput outputStream;

        public uint SessionId { get; private set; }

        private uint clientCrcLength { get; set; }
        private byte serverCrcLength { get; set; } = 2;
        private uint clientUdpLength { get; set; }
        private uint serverUdpLength { get; set; } = 512u;
        private ushort serverCompression { get; set; } = 0x100;

        private bool hasAuthed = false;

        public GameSession(string cryptoKey)
        {
            EncryptionKey = cryptoKey;

            inputStream = new DataStreamInput(this);
            inputStream.OnData += (gamePacket) =>
            {
                OnGamePacket(gamePacket);
            };
            inputStream.OnOutOfOrder += (sequence) =>
            {
                EnqueueProtocolMessage(new OutOfOrder
                {
                    Sequence = sequence
                },
                new PacketOptions
                {
                    Compression = serverCompression != 0
                });
            };
            outputStream = new DataStreamOutput(this);
            outputStream.OnData += (sequence, gamePacket) =>
            {
                if (!gamePacket.IsFragment)
                    EnqueueProtocolMessage(new DataWhole
                    {
                        Sequence = sequence,
                        Data = gamePacket.Data
                    }, new PacketOptions
                    {
                        IsSubpacket = false,
                        Compression = serverCompression != 0
                    });
                else
                    EnqueueProtocolMessage(new DataFragment
                    {
                        Sequence = sequence,
                        Data = gamePacket.Data
                    }, new PacketOptions
                    {
                        IsSubpacket = false,
                        Compression = serverCompression != 0
                    });
            };
        }

        /// <summary>
        /// This is fired to alert the Class inheritor that a packet is here for it to handle.
        /// </summary>
        /// <remarks>GameSession only handles Protocol packets directly. AuthSession, WorldSession, and any others would handle Game Packets.</remarks>
        protected virtual void OnGamePacket(byte[] data)
        {
            // deliberately empty
        }

        /// <summary>
        /// This is fired between incoming packets being parsed and outgoing packets being sent, to allow for the Class inheritor to handle its packets.
        /// </summary>
        /// <remarks>This is called from within Update().</remarks>
        protected virtual void OnProcessPackets(double lastTick)
        {
            // deliberately empty
        }

        public override void OnData(byte[] data)
        {
            Heartbeat.OnHeartbeat();

            var packet = new ProtocolPacket(data, new PacketOptions
            {
                IsSubpacket = false,
                Compression = serverCompression != 0
            });
            incomingPackets.Enqueue(packet);

            base.OnData(data);
        }

        public override void OnDisconnect()
        {
            SendPacket(new ProtocolPacket(ProtocolMessageOpcode.Disconnect, new Disconnect(), true, new PacketOptions()));
            base.OnDisconnect();
        }

        /// <summary>
        /// Invoked each server tick with the delta since the previous tick occurred.
        /// </summary>
        public sealed override void Update(double lastTick)
        {
            // process pending packet queue
            while (CanProcessPackets && incomingPackets.TryDequeue(out ProtocolPacket packet))
                HandleProtocolPacket(packet);

            OnProcessPackets(lastTick);

            if (CanProcessPackets && outgoingPackets.TryDequeue(out ProtocolPacket outPacket))
                SendPacket(outPacket);

            base.Update(lastTick);
        }

        private void HandleProtocolPacket(ProtocolPacket packet)
        {
            IProtocol message = MessageManager.Instance.GetProtocolMessage(packet.Opcode);
            if (message == null)
            {
                log.Warn($"Received unknown protocol packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            if (!hasAuthed && message is not SessionRequest)
            {
                OnDisconnect();
                return;
            }

            MessageHandlerDelegate handlerInfo = MessageManager.Instance.GetProtocolMessageHandler(packet.Opcode);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled protocol packet {packet.Opcode}(0x{packet.Opcode:X}).");
                return;
            }

            log.Trace($"Received packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data)}");

            var reader = new ProtocolPacketReader(packet.Data);

            message.Read(reader, packet.PacketOptions);
            if (reader.BytesRemaining > 2)
                log.Warn($"Failed to read entire contents of packet {packet.Opcode} ({reader.BytesRemaining} bytes remaining)");

            try
            {
                handlerInfo.Invoke(this, message);
            }
            catch (InvalidPacketValueException exception)
            {
                log.Error(exception);
                OnDisconnect();
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        public void EnqueueProtocolMessage(IProtocol message, PacketOptions options)
        {
            if (!MessageManager.Instance.GetOpcodeData(message, out (ProtocolMessageOpcode, bool) opcodeData))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new ProtocolPacket(opcodeData.Item1, message, opcodeData.Item2, options));
        }

        protected void SendPacket(ProtocolPacket packet)
        {
            List<byte> data = new();
            var writer = new ProtocolPacketWriter(data);

            writer.Write((ushort)packet.Opcode);
            writer.WriteBytes(packet.Data);

            byte[] newData = data.ToArray();
            if (packet.UseEncryption)
                newData = CrcProvider.AppendCRC(newData, 0);

            log.Trace($"Sending packet {packet.Opcode}(0x{packet.Opcode:X})");

            SendRaw(newData);
        }

        protected void PackAndSend(byte[] data)
        {
            // Send to Output Stream for Packing
            // Output Stream will emit the packed data to be added to outgoingPackets Queue to be sent.
            outputStream.PackData(data);
        }

        protected void ToggleEncryption(bool usingEncryption)
        {
            inputStream.SetEncryption(usingEncryption);
            outputStream.SetEncryption(usingEncryption);
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.SessionRequest)]
        public void HandleSessionRequest(SessionRequest request)
        {
            log.Info($"{request.SessionId}, {request.CRCLength}, {request.UdpLength}, {request.Protocol}");

            hasAuthed = true;

            SessionId       = request.SessionId;
            clientCrcLength = request.CRCLength;
            clientUdpLength = request.UdpLength;

            if (clientUdpLength != 512)
                throw new InvalidOperationException();

            if (request.Protocol == "LoginUdp_10")
                ProtocolVersion = ProtocolVersion.LoginUdp_10;
            if (request.Protocol == "LoginUdp_9")
                ProtocolVersion = ProtocolVersion.LoginUdp_9;
            if (request.Protocol == "ExternalGatewayApi_3")
            {
                ProtocolVersion = ProtocolVersion.ExternalGatewayApi_3;
                serverCompression = 0x0;
            }

            // Leave room for Oopcode, Compression Byte, and CRC
            outputStream.SetFragmentSize(clientUdpLength - 7);
            EnqueueProtocolMessage(new SessionReply
            {
                SessionId   = SessionId,
                CRCSeed     = 0,
                CRCLength   = serverCrcLength,
                Compression = serverCompression,
                UdpLength   = serverUdpLength
            }, new PacketOptions
            {
                IsSubpacket = false,
                Compression = serverCompression != 0
            });
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.DataFragment)]
        public void HandleDataFragment(DataFragment dataFragment)
        {
            inputStream.ProcessDataFragment(dataFragment);
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.Data)]
        public void HandleDataFragment(DataWhole dataWhole)
        {
            inputStream.ProcessDataFragment(dataWhole);
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.MutliPacket)]
        public void HandleMultiPacket(MultiPacket multiPacket)
        {
            foreach (ProtocolPacket packet in multiPacket.Packets)
                incomingPackets.Enqueue(packet);
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.Ping)]
        public void HandlePing(Ping ping)
        {
            EnqueueProtocolMessage(new Ping(), new PacketOptions());
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.Disconnect)]
        public void HandleDisconnect(Disconnect disconnect)
        {
            OnDisconnect();
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.Ack)]
        public void HandleAck(Ack ack)
        {
            outputStream.HandleAck(ack.Sequence);
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.OutOfOrder)]
        public void HandleOutOfOrder(OutOfOrder outOfOrder)
        {
            outputStream.ResendData(outOfOrder.Sequence);
        }
    }
}
