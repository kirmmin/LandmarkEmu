using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Database.Character.Model;
using LandmarkEmulator.Gateway.Network.Message;
using LandmarkEmulator.Gateway.Network.Packets;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace LandmarkEmulator.Gateway.Network
{
    public class GatewaySession : GameSession
    {
        public AccountModel Account { get; private set; }
        public List<CharacterModel> Characters { get; } = new();

        private readonly ConcurrentQueue<GatewayPacket> incomingPackets = new();
        private readonly Queue<GatewayPacket> outgoingPackets = new();

        public GatewaySession(string cryptoKey) : base(cryptoKey)
        {
        }

        public override void OnAccept(EndPoint ep)
        {
            base.OnAccept(ep);

            log.Debug($"New session received on {ep}");
        }

        protected override void OnGamePacket(byte[] data)
        {
            base.OnGamePacket(data);

            var packet = new GatewayPacket(data);
            incomingPackets.Enqueue(packet);
        }

        protected override void OnProcessPackets(double lastTick)
        {
            while (CanProcessPackets && incomingPackets.TryDequeue(out GatewayPacket packet))
                HandleAuthPacket(packet);

            while (CanProcessPackets && outgoingPackets.TryDequeue(out GatewayPacket packet))
                SendPacket(packet);
        }

        private void HandleAuthPacket(GatewayPacket packet)
        {
            IReadable message = GatewayMessageManager.Instance.GetAuthMessage(packet.Opcode, ProtocolVersion);
            if (message == null)
            {
                log.Warn($"Received unknown Auth packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            GatewayMessageHandlerDelegate handlerInfo = GatewayMessageManager.Instance.GetGameMessageHandler(packet.Opcode, ProtocolVersion);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled Auth packet {packet.Opcode}(0x{packet.Opcode:X})  : {BitConverter.ToString(packet.Data)}.");
                return;
            }

            log.Debug($"Received packet {packet.Opcode}(0x{packet.Opcode:X})  : {BitConverter.ToString(packet.Data)}");

            var reader = new GamePacketReader(packet.Data);

            message.Read(reader);
            if (reader.BytesRemaining > 0)
                log.Warn($"Failed to read entire contents of Auth packet {packet.Opcode} ({reader.BytesRemaining} bytes remaining)");

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

        private void SendPacket(GatewayPacket packet)
        {
            List<byte> data = new();
            var writer = new GamePacketWriter(data);

            writer.Write((byte)packet.Opcode);
            writer.WriteBytes(packet.Data);

            log.Debug($"Sending packet {packet.Opcode}(0x{packet.Opcode:X})");

            PackAndSend(data.ToArray());
        }

        public void EnqueueMessage(IWritable message)
        {
            if (!GatewayMessageManager.Instance.GetOpcode(message, out GatewayMessageOpcode opcode))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new GatewayPacket(opcode, message));
        }
    }
}
