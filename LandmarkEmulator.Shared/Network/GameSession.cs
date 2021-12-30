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

        private readonly ConcurrentQueue<ProtocolPacket> incomingPackets = new();
        private readonly Queue<ProtocolPacket> outgoingPackets = new();
        protected readonly DataPacket[] DataPackets = new DataPacket[ushort.MaxValue];

        public uint SessionId { get; private set; }

        private uint clientCrcLength { get; set; }
        private byte serverCrcLength { get; set; } = 2;
        private uint clientUdpLength { get; set; }
        private uint serverUdpLength { get; set; } = 512u;
        private ushort serverCompression { get; set; } = 0x100;

        /// <summary>
        /// Used to track Ack packet sequences.
        /// </summary>
        protected ushort? LastAck { get; set; } = null;
        protected ushort? NextAck { get; set; } = null;

        /// <summary>
        /// Used to track data packet sequences.
        /// </summary>
        protected ushort? LastSequence { get; set; } = null;
        protected ushort? NextSequence { get; set; } = null;

        public override void OnData(byte[] data)
        {
            Heartbeat.OnHeartbeat();

            var packet = new ProtocolPacket(data);
            incomingPackets.Enqueue(packet);

            base.OnData(data);
        }

        protected override void OnDisconnect()
        {
            base.OnDisconnect();
        }

        public override void Update(double lastTick)
        {
            // process pending packet queue
            while (CanProcessPackets && incomingPackets.TryDequeue(out ProtocolPacket packet))
                HandleProtocolPacket(packet);

            while (CanProcessPackets && outgoingPackets.TryDequeue(out ProtocolPacket packet))
                SendPacket(packet);

            base.Update(lastTick);
        }

        private void HandleProtocolPacket(ProtocolPacket packet)
        {
            IReadable message = MessageManager.GetProtocolMessage(packet.Opcode);
            if (message == null)
            {
                log.Warn($"Received unknown packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            MessageHandlerDelegate handlerInfo = MessageManager.GetProtocolMessageHandler(packet.Opcode);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled packet {packet.Opcode}(0x{packet.Opcode:X}).");
                return;
            }

            log.Trace($"Received packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data).Replace("-", "")}");

            var reader = new GamePacketReader(packet.Data);

            message.Read(reader);
            if (reader.BytesRemaining > 0)
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

        public void EnqueueMessage(IWritable message)
        {
            if (!MessageManager.GetOpcodeData(message, out (ProtocolMessageOpcode, bool) opcodeData))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new ProtocolPacket(opcodeData.Item1, message, opcodeData.Item2));
        }

        protected void SendPacket(ProtocolPacket packet)
        {
            List<byte> data = new();
            var writer = new GamePacketWriter(data);

            writer.Write((ushort)packet.Opcode);
            writer.WriteBytes(packet.Data);

            byte[] newData = data.ToArray();
            if (packet.UseEncryption)
                newData = Encryption.AppendCRC(newData, 0);

            log.Trace($"Sending packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(newData)}");

            SendRaw(newData);
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.SessionRequest)]
        public void HandleSessionRequest(SessionRequest request)
        {
            log.Info($"{request.SessionId}, {request.CRCLength}, {request.UdpLength}, {request.Protocol}");

            SessionId       = request.SessionId;
            clientCrcLength = request.CRCLength;
            clientUdpLength = request.UdpLength;

            if (clientUdpLength != 512)
                throw new InvalidOperationException();

            if (request.Protocol != "LoginUdp_10")
                throw new InvalidOperationException();

            EnqueueMessage(new SessionReply
            {
                SessionId   = SessionId,
                CRCSeed     = 0,
                CRCLength   = serverCrcLength,
                Compression = serverCompression,
                UdpLength   = serverUdpLength
            });
        }

        [ProtocolMessageHandler(ProtocolMessageOpcode.DataFragment)]
        public void HandleDataFragment(DataFragment dataFragment)
        {
            log.Info($"{dataFragment.Sequence}, {dataFragment.CRC}");

            if (NextSequence == null)
                NextSequence = dataFragment.Sequence;

            if (dataFragment.Sequence > NextSequence)
            {
                log.Warn($"Sequence out of order, expected {NextSequence} but received {dataFragment.Sequence}");
                OnDisconnect();

                // TODO: Handle out of order packets
                // this.emit("outoforder", null, this._nextSequence, sequence);

                throw new InvalidOperationException();
            }

            int lastAck = LastAck ?? -1;
            ushort ack = dataFragment.Sequence;
            for (ushort i = 1; i <= ushort.MaxValue; i++)
            {
                ushort j = (ushort)((lastAck + i) & 0xFFFF);
                if (DataPackets[j] != null)
                    ack = j;
                else
                    break;
            }

            if (ack > lastAck)
            {
                LastAck = ack;
                EnqueueMessage(new Ack
                {
                    Sequence = ack
                });
                log.Warn($"FIRE ACK {LastAck}");
            }

            DataPackets[dataFragment.Sequence] = new DataPacket(dataFragment.Data, true);

            NextSequence = (ushort)((LastAck + 1) & 0xFFFF);
        }
    }

}
