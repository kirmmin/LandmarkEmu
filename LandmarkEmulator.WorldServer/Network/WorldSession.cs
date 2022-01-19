using LandmarkEmulator.Database.Character.Model;
using LandmarkEmulator.Gateway.Network;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message.Model;
using LandmarkEmulator.WorldServer.Network.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LandmarkEmulator.WorldServer.Network
{
    public class WorldSession : GatewaySession
    {
        private readonly ConcurrentQueue<ZonePacket> incomingPackets = new();
        private readonly Queue<ZonePacket> outgoingPackets = new();

        public WorldSession() : base(WorldServer.EncryptionKey)
        {
        }

        protected override void OnProcessZonePackets(double lastTick)
        {
            while (CanProcessPackets && incomingPackets.TryDequeue(out ZonePacket packet))
                HandleZonePacket(packet);

            while (CanProcessPackets && outgoingPackets.TryDequeue(out ZonePacket packet))
                SendPacket(packet);
        }

        private void HandleZonePacket(ZonePacket packet)
        {
            IReadable message = ZoneMessageManager.Instance.GetZoneMessage(packet.Opcode, ClientProtocol);
            if (message == null)
            {
                log.Warn($"Received unknown Zone packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            ZoneMessageHandlerDelegate handlerInfo = ZoneMessageManager.Instance.GetZoneMessageHandler(packet.Opcode, ClientProtocol);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled Zone packet {packet.Opcode}(0x{packet.Opcode:X}).");
                return;
            }

            log.Debug($"Received packet {packet.Opcode}(0x{packet.Opcode:X})");

            var reader = new GamePacketReader(packet.Data);

            message.Read(reader);
            if (reader.BytesRemaining > 0)
                log.Warn($"Failed to read entire contents of Zone packet {packet.Opcode} ({reader.BytesRemaining} bytes remaining)");

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

        public override void OnCharacterLogin(CharacterModel model)
        {
            base.OnCharacterLogin(model);

            // Setup Player, Send Packets, etc.
            log.Info($"Setting up player, etc.");
            EnqueueMessage(new SendSelfToClient());
        }

        public override void OnTunnelData(int flags, byte[] data)
        {
            (ZoneMessageOpcode? opcode, int offset) = GetOpcode(data);
            data = new Span<byte>(data, offset, data.Length - offset).ToArray();

            if (opcode == null)
            {
                log.Warn($"Unknown Zone Packet : {BitConverter.ToString(data)}");
                return;
            }

            var packet = new ZonePacket((ZoneMessageOpcode)opcode, data);
            incomingPackets.Enqueue(packet);
        }

        private byte[] GetOpcodeBytes(ZoneMessageOpcode? opcode)
        {
            var hexOpcode = $"{opcode:X}";
            int position = 0;
            List<byte> hexBytes = new();
            while (position < hexOpcode.Length)
            {
                var opByte = hexOpcode.Substring(position, 2);
                byte num = byte.Parse(opByte, System.Globalization.NumberStyles.AllowHexSpecifier);

                if (hexBytes.Count == 0 && num == 0)
                {
                    position += 2;
                    continue;
                }

                hexBytes.Add(num);
                position += 2;
            }

            return hexBytes.ToArray();
        }

        private (ZoneMessageOpcode?, int) GetOpcode(byte[] data)
        {
            if (Enum.IsDefined(typeof(ZoneMessageOpcode), (int)(data[0])))
                return ((ZoneMessageOpcode)data[0], 2);
            
            if (data.Length > 1)
            {
                var opcode = (data[0] << 8) + data[1];
                if (Enum.IsDefined(typeof(ZoneMessageOpcode), opcode))
                    return ((ZoneMessageOpcode)opcode, 2);

                if (data.Length > 2)
                {
                    opcode = (data[0] << 16) + (data[1] << 8) + data[2];
                    if (Enum.IsDefined(typeof(ZoneMessageOpcode), opcode))
                        return ((ZoneMessageOpcode)opcode, 3);

                    if (data.Length > 3)
                    {
                        opcode = (data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3];
                        if (Enum.IsDefined(typeof(ZoneMessageOpcode), opcode))
                            return ((ZoneMessageOpcode)opcode, 4);
                    }
                }
            }

            return (null, 0);
        }

        public new void EnqueueMessage(IWritable message)
        {
            if (!ZoneMessageManager.Instance.GetOpcode(message, out ZoneMessageOpcode opcode))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new ZonePacket(opcode, message));
        }

        private void SendPacket(ZonePacket packet)
        {
            List<byte> data = new();
            var writer = new GamePacketWriter(data);

            writer.WriteBytes(GetOpcodeBytes(packet.Opcode));
            writer.WriteBytes(packet.Data);

            log.Debug($"Sending packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data)}");

            PackTunnelPacket(data.ToArray());
        }
    }
}
