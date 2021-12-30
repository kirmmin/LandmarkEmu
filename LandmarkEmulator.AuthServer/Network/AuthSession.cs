﻿using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Network.Packets;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Concurrent;
using System.Net;

namespace LandmarkEmulator.AuthServer.Network
{
    public class AuthSession : GameSession
    {
        private readonly ConcurrentQueue<AuthPacket> incomingPackets = new();

        public override void OnAccept(EndPoint ep)
        {
            base.OnAccept(ep);

            log.Trace($"New session received on {ep.ToString()}");
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
        }

        private void HandleAuthPacket(AuthPacket packet)
        {
            IReadable message = AuthMessageManager.GetAuthMessage(packet.Opcode);
            if (message == null)
            {
                log.Warn($"Received unknown Auth packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            AuthMessageHandlerDelegate handlerInfo = AuthMessageManager.GetGameMessageHandler(packet.Opcode);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled Auth packet {packet.Opcode}(0x{packet.Opcode:X}).");
                return;
            }

            log.Trace($"Received packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data).Replace("-", "")}");

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
    }
}
