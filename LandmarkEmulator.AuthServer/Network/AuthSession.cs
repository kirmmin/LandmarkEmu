using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Network.Packets;
using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace LandmarkEmulator.AuthServer.Network
{
    public class AuthSession : GameSession
    {
        public AccountModel Account { get; private set; }

        private readonly ConcurrentQueue<AuthPacket> incomingPackets = new();
        private readonly Queue<AuthPacket> outgoingPackets = new();

        /// <summary>
        /// Initialise an <see cref="AuthSession"/> from an existing <see cref="AccountModel"/>.
        /// </summary>
        public void Initialise(AccountModel model)
        {
            if (Account != null)
                throw new InvalidOperationException();

            Account = model;
        }

        public override void OnAccept(EndPoint ep)
        {
            base.OnAccept(ep);

            log.Debug($"New session received on {ep}");
        }

        protected override void OnGamePacket(byte[] data)
        {
            base.OnGamePacket(data);
            
            var packet = new AuthPacket(data);
            incomingPackets.Enqueue(packet);
        }

        protected override void OnProcessPackets(double lastTick)
        {
            while (CanProcessPackets && incomingPackets.TryDequeue(out AuthPacket packet))
                HandleAuthPacket(packet);

            while (CanProcessPackets && outgoingPackets.TryDequeue(out AuthPacket packet))
                SendPacket(packet);
        }

        private void HandleAuthPacket(AuthPacket packet)
        {
            IReadable message = AuthMessageManager.Instance.GetAuthMessage(packet.Opcode, ProtocolVersion);
            if (message == null)
            {
                log.Warn($"Received unknown Auth packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            AuthMessageHandlerDelegate handlerInfo = AuthMessageManager.Instance.GetGameMessageHandler(packet.Opcode, ProtocolVersion);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled Auth packet {packet.Opcode}(0x{packet.Opcode:X}).");
                return;
            }

            log.Debug($"Received packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data).Replace("-", "")}");

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

        private void SendPacket(AuthPacket packet)
        {
            List<byte> data = new();
            var writer = new GamePacketWriter(data);

            writer.Write((byte)packet.Opcode);
            writer.WriteBytes(packet.Data);

            log.Debug($"Sending packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(data.ToArray())}");

            PackAndSend(data.ToArray());
        }

        public void EnqueueMessage(IWritable message)
        {
            if (!AuthMessageManager.Instance.GetOpcode(message, out AuthMessageOpcode opcode))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new AuthPacket(opcode, message));
        }
    }
}
