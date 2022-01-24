using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Database.Character.Model;
using LandmarkEmulator.Gateway.Network.Message;
using LandmarkEmulator.Gateway.Network.Message.Model;
using LandmarkEmulator.Gateway.Network.Packets;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.Game.Events;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LandmarkEmulator.Gateway.Network
{
    public class GatewaySession : GameSession
    {
        public AccountModel Account { get; private set; }
        public List<CharacterModel> Characters { get; private set; }

        private readonly ConcurrentQueue<GatewayPacket> incomingPackets = new();
        private readonly Queue<GatewayPacket> outgoingPackets = new();

        public ClientProtocol ClientProtocol { get; private set; }
        public string ClientBuild { get; private set; }

        public GatewaySession(string cryptoKey) : base(cryptoKey)
        {
            // We initialise with Arc4 Encryption off, because the initial Data Packet which includes the LoginRequest comes in un-encrypted.
            ToggleEncryption(false);
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

        /// <summary>
        /// This is fired between incoming packets being parsed and outgoing packets being sent, to allow for the Class inheritor to handle its packets.
        /// </summary>
        /// <remarks>This is called from within Update().</remarks>
        protected virtual void OnProcessZonePackets(double lastTick)
        {
            // Deliberately left empty
        }

        protected sealed override void OnProcessPackets(double lastTick)
        {
            while (CanProcessPackets && incomingPackets.TryDequeue(out GatewayPacket packet))
                HandlePacket(packet);

            OnProcessZonePackets(lastTick);

            while (CanProcessPackets && outgoingPackets.TryDequeue(out GatewayPacket packet))
                SendPacket(packet);
        }

        private void HandlePacket(GatewayPacket packet)
        {
            // Handle Tunnel Packets slightly separately.
            if (packet.Opcode == GatewayMessageOpcode.TunnelPacketFromExternalConnection)
            {
                log.Debug($"Received Gateway packet {packet.Opcode}(0x{packet.Opcode:X})  : {BitConverter.ToString(packet.Data)}");
                OnTunnelData(packet.Flags, packet.Data);
                return;
            }

            IReadable message = GatewayMessageManager.Instance.GetGatewayMessage(packet.Opcode, ProtocolVersion);
            if (message == null)
            {
                log.Warn($"Received unknown Gateway packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            GatewayMessageHandlerDelegate handlerInfo = GatewayMessageManager.Instance.GetGatewayMessageHandler(packet.Opcode, ProtocolVersion);
            if (handlerInfo == null)
            {
                log.Warn($"Received unhandled Gateway packet {packet.Opcode}(0x{packet.Opcode:X})  : {BitConverter.ToString(packet.Data)}.");
                return;
            }

            log.Debug($"Received packet {packet.Opcode}(0x{packet.Opcode:X})  : {BitConverter.ToString(packet.Data)}");

            var reader = new GamePacketReader(packet.Data);
            message.Read(reader);
            if (reader.BytesRemaining > 0)
                log.Warn($"Failed to read entire contents of Gateway packet {packet.Opcode} ({reader.BytesRemaining} bytes remaining)");

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

            if (packet.Opcode == GatewayMessageOpcode.TunnelPacketToExternalConnection)
                writer.Write((byte)((int)packet.Opcode | packet.Flags << 5));
            else
                writer.Write((byte)packet.Opcode);
            writer.WriteBytes(packet.Data);

            log.Debug($"Sending packet {packet.Opcode}(0x{packet.Opcode:X}) : {BitConverter.ToString(packet.Data)}");

            PackAndSend(data.ToArray());
        }

        public void EnqueueGatewayMessage(IWritable message)
        {
            if (!GatewayMessageManager.Instance.GetOpcode(message, out GatewayMessageOpcode opcode))
            {
                log.Warn("Failed to send message with no attribute!");
                return;
            }

            outgoingPackets.Enqueue(new GatewayPacket(opcode, message));
        }

        [GatewayMessageHandler(GatewayMessageOpcode.LoginRequest, ProtocolVersion.ExternalGatewayApi_3)]
        public void HandleLoginRequest(LoginRequest request)
        {
            log.Info($"{request.CharacterId}, {request.ServerTicket}, {request.ClientProtocol}, {request.ClientBuild}");

            // We toggle Encrytion back on, here, because the Client expects the LoginReply (and all subsequent packets) to come in with Arc4 Encryption.
            ToggleEncryption(true);

            ClientBuild = request.ClientBuild;
            if (Enum.TryParse(request.ClientProtocol, out ClientProtocol protocol))
                ClientProtocol = protocol;
            else
            {
                log.Error($"Unsupported Client Protocol: {request.ClientProtocol}. Disconnecting.");
                OnDisconnect();
                return;
            }

            Events.Enqueue(new TaskGenericEvent<AccountModel>(DatabaseManager.Instance.AuthDatabase.GetAccountByServerTicketAsync(request.ServerTicket),
                account =>
            {
                bool loggedIn = false;
                if (account != null)
                    loggedIn = true;

                EnqueueGatewayMessage(new LoginReply
                {
                    LoggedIn = loggedIn
                });

                Account = account;
                Events.Enqueue(new TaskGenericEvent<List<CharacterModel>>(DatabaseManager.Instance.CharacterDatabase.GetCharacters(Account.Id),
                    characters =>
                {
                    Characters = characters;

                    var selectedCharacter = Characters.SingleOrDefault(x => x.Id == request.CharacterId);
                    if (selectedCharacter != null)
                        OnCharacterLogin(selectedCharacter);
                    else
                    {
                        log.Error($"Account {Account.Id} tried to login as Character {request.CharacterId}. Potential hack attempt. Disconnecting.");
                        OnDisconnect();
                    }
                }));
            }));
        }

        /// <summary>
        /// Called after a user's login has been authenticated to indiciate to inherited Classes which character is logging in.
        /// </summary>
        public virtual void OnCharacterLogin(CharacterModel model)
        {
            // Deliberately left empty
        }

        /// <summary>
        /// Called when the Gateway receives a <see cref="GatewayMessageOpcode.TunnelPacketFromExternalConnection"/> message.
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnTunnelData(int flags, byte[] data)
        {
            // Deliberately left empty
        }

        /// <summary>
        /// Packs the provided <see cref="byte[]"/> into a Tunnel Packet to be sent to the Client.
        /// </summary>
        protected void PackTunnelPacket(byte[] data)
        {
            EnqueueGatewayMessage(new TunnelPacketToExternalConnection
            {
                Data = data
            });
        }
    }
}
