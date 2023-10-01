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
using System.IO;
using System.Linq;

namespace LandmarkEmulator.WorldServer.Network
{
    public class WorldSession : GatewaySession
    {
        private readonly Queue<ZonePacket> outgoingPackets = new();

        private byte[] sendSelfPacket;
        public byte[] TerrainManifest { get; }
        public bool ReadySent { get; set; } = false;

        public WorldSession() : base(WorldServer.EncryptionKey)
        {
            sendSelfPacket = File.ReadAllBytes("Resources\\PacketDumps\\SendSelf.bin");
            TerrainManifest = File.ReadAllBytes("Resources\\PacketDumps\\TerrainCellManifest.bin");
        }

        #region Session Handlers
        public override void OnCharacterLogin(CharacterModel model)
        {
            base.OnCharacterLogin(model);
            //EnqueueMessage((ZoneMessageOpcode)0x16015A04, File.ReadAllBytes("packets\\25-unknown").AsSpan().Slice(4).ToArray());
            EnqueueMessage(ZoneMessageOpcode.SendZoneDetails, Convert.FromHexString("14-00-00-00-4F-70-65-6E-42-65-74-61-47-65-6F-6D-65-74-72-79-5F-34-30-36-05-00-00-00-00-07-00-00-00-53-6B-79-2E-78-6D-6C-00-00-00-00-96-01-00-00-FF-FF-FF-FF-FF-FF-FF-FF-96-01-00-00-02-00-00-00-10-8F-08-00-96-01-00-00-05-00-00-00-4C-B8-08-00-15-00-00-00-4F-70-65-6E-42-65-74-61-43-6F-6E-74-69-6E-65-6E-74-5F-34-30-36-21-00-00-00-4F-70-65-6E-42-65-74-61-5F-44-6F-5F-4E-4F-54-5F-43-68-61-6E-67-65-54-68-69-73-54-65-78-74-5F-31-36-00-00-00-00-00-00-44-40-00-00-00-00-E0-84-43-41-00-00-00-00-00-40-9F-40-00-00-00-00-11-81-43-41-00-00-00-00-00-40-BF-40-00-00-00-00-00-40-AF-40-00-00-00-00-00-40-BF-40-01-00-1A-00-00-00-43-6F-6E-74-69-6E-65-6E-74-5F-42-69-6F-6D-65-49-64-73-5F-34-30-36-2E-62-6D-70-CD-CC-CC-40-17-00-00-00-43-6F-6E-74-69-6E-65-6E-74-5F-53-68-61-70-65-5F-34-30-36-2E-62-6D-70-CD-CC-CC-40-20-00-00-00-43-6F-6E-74-69-6E-65-6E-74-5F-48-65-69-67-68-74-4C-61-79-65-72-49-64-73-5F-34-30-36-2E-62-6D-70-00-00-C8-43-10-27-00-00-00-00-80-3E-00-00-80-3E-00-00-80-3E-00-00-40-3F-00-00-00-3F-00-00-00-40-03-00-00-00-0A-D7-23-3C-00-40-9C-44-00-C0-41-45-00-00-F0-41-01-00-00-00-00-00-00-00-00-6E-A7-40-DA-01-00-00-00-00-00-00-00-B5-36-00-00-01-00-00-80-3E-CD-CC-CC-3D-01-00-00-00-00-00-00-00-00".Replace("-", "")));

            // Setup Player, Send Packets, etc.
            EnqueueMessage(ZoneMessageOpcode.InitializationParameters, Convert.FromHexString("04-00-00-00-6C-69-76-65".Replace("-", "")));
            EnqueueMessage(ZoneMessageOpcode.ClientGameSettings, Convert.FromHexString("00-00-00-00-0A-00-00-00-00-00-00-00-00-00-00-80-3F-00-00-00-00-01-00-00-00-00-00-00-00-00-00-40-41-00-00-DC-42".Replace("-", "")));
            EnqueueMessage(ZoneMessageOpcode.ReferenceDataItemClassDefinitions, Convert.FromHexString("00-00-00-00".Replace("-", "")));
            EnqueueMessage(ZoneMessageOpcode.ReferenceDataItemCategoryDefinitions, File.ReadAllBytes("Resources\\PacketDumps\\39-ReferenceDataItemCategoryDefinitions").AsSpan().Slice(4).ToArray());
            EnqueueMessage(ZoneMessageOpcode.ReferenceDataProfileDefinitions, File.ReadAllBytes("Resources\\PacketDumps\\41-ReferenceDataProfileDefinitions").AsSpan().Slice(4).ToArray());
            EnqueueMessage(ZoneMessageOpcode.SkillBase, Convert.FromHexString("05-02-00-00-00-00-00-00-00-04-00-00-00-01-00-00-00-01-00-00-00-44-82-08-00-44-82-08-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-05-00-00-00-05-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-03-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-02-00-00-00-02-00-00-00-44-82-08-00-44-82-08-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-01-00-00-00-05-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-04-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-03-00-00-00-03-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-02-00-00-00-0F-00-00-00-0A-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-05-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-04-00-00-00-04-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-03-00-00-00-1E-00-00-00-0F-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-06-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-01-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-01-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-01-00-00-00-01-00-00-00-01-00-00-00-44-82-08-00-44-82-08-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-08-00-00-00-13-00-00-00-13-00-00-00-01-00-00-00-00-00-00-00-14-00-00-00-14-00-00-00-01-00-00-00-00-00-00-00-1C-00-00-00-1C-00-00-00-01-00-00-00-00-00-00-00-1D-00-00-00-1D-00-00-00-01-00-00-00-00-00-00-00-1E-00-00-00-1E-00-00-00-01-00-00-00-00-00-00-00-1F-00-00-00-1F-00-00-00-01-00-00-00-00-00-00-00-20-00-00-00-20-00-00-00-01-00-00-00-00-00-00-00-21-00-00-00-21-00-00-00-01-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00".Replace("-", "")));
            EnqueueMessage(ZoneMessageOpcode.CommandBase, File.ReadAllBytes("Resources\\PacketDumps\\44-CommandBase").AsSpan().Slice(2).ToArray());
            EnqueueMessage(ZoneMessageOpcode.SendSelfToClient, sendSelfPacket.AsSpan().Slice(2).ToArray());
        }
        #endregion

        #region Packet Management
        protected override void OnProcessZonePackets()
        {
            while (CanProcessPackets && outgoingPackets.TryDequeue(out ZonePacket packet))
                SendPacket(packet);
        }

        private void HandleZonePacket(ZonePacket packet)
        {
            IReadable message = ZoneMessageManager.Instance.GetZoneMessage(packet.Opcode, ClientProtocol);
            if (message == null)
            {
                log.Warn($"Received Zone packet that was unreadable {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data)}");
                return;
            }

            log.Debug($"Received Zone packet {packet.Opcode}(0x{packet.Opcode:X})");

            var reader = new GamePacketReader(packet.Data);
            message.Read(reader);
            if (reader.BytesRemaining > 0)
                log.Warn($"Failed to read entire contents of Zone packet {packet.Opcode} ({reader.BytesRemaining} bytes remaining)");

            ZoneMessageHandlerDelegate handlerInfo = ZoneMessageManager.Instance.GetZoneMessageHandler(packet.Opcode, ClientProtocol);
            if (handlerInfo == null)
            {
                log.Warn($"Unhandled Zone packet {packet.Opcode}(0x{packet.Opcode:X}).");
                return;
            }

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

        public static (ZoneMessageOpcode?, int) GetOpcode(byte[] data)
        {
            if (Enum.IsDefined(typeof(ZoneMessageOpcode), (uint)(data[0])))
                return ((ZoneMessageOpcode)data[0], 2);
            
            if (data.Length > 1)
            {
                uint opcode = (uint)((data[0] << 8) + data[1]);
                if (Enum.IsDefined(typeof(ZoneMessageOpcode), opcode))
                    return ((ZoneMessageOpcode)opcode, 2);

                if (data.Length > 2)
                {
                    opcode = (uint)((data[0] << 16) + (data[1] << 8) + data[2]);
                    if (Enum.IsDefined(typeof(ZoneMessageOpcode), opcode))
                        return ((ZoneMessageOpcode)opcode, 3);

                    if (data.Length > 3)
                    {
                        opcode = (uint)((data[0] << 24) + (data[1] << 16) + (data[2] << 8) + data[3]);
                        if (Enum.IsDefined(typeof(ZoneMessageOpcode), opcode))
                            return ((ZoneMessageOpcode)opcode, 4);
                    }
                }
            }

            return (null, 0);
        }

        public void EnqueueMessage(IWritable message)
        {
            if (!ZoneMessageManager.Instance.GetOpcode(message, out ZoneMessageOpcode opcode))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new ZonePacket(opcode, message));
        }

        public void EnqueueMessage(ZoneMessageOpcode opcode, byte[] data)
        {
            outgoingPackets.Enqueue(new ZonePacket(opcode, data));
        }

        private void SendPacket(ZonePacket packet)
        {
            List<byte> data = new();
            var writer = new GamePacketWriter(data);

            writer.WriteBytes(GetOpcodeBytes(packet.Opcode));
            writer.WriteBytes(packet.Data);

            log.Debug($"Sending packet {packet.Opcode}(0x{packet.Opcode:X})");
            log.Trace($"{BitConverter.ToString(packet.Data)}");

            PackTunnelPacket(data.ToArray());
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
            HandleZonePacket(packet);
        }
        #endregion
    }
}
