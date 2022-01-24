using Haukcode.PcapngUtils;
using Haukcode.PcapngUtils.Common;
using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Network.Message.Model;
using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.Gateway.Network.Message;
using LandmarkEmulator.Shared.GameTable.Text;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.Shared.Network.Message.Model;
using LandmarkEmulator.Shared.Network.Packets;
using LandmarkEmulator.WorldServer.Network;
using LandmarkEmulator.WorldServer.Network.Message;
using LandmarkEmulator.WorldServer.Network.Packets;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LandmarkEmulator.Parser
{
    public class Parser
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private static DataStreamInput authServerStream;
        private static DataStreamInput authClientStream;
        private static DataStreamInput zoneServerStream;
        private static DataStreamInput zoneClientStream;

        public enum Protocol
        {
            TCP = 6,
            UDP = 17,
            Unknown = -1
        };

        public enum GamePacketType
        {
            Auth,
            Gateway,
            Zone
        };

        private static string ClientHeader = "[Client]";
        private static string ServerHeader = "[Server]";
        private static string FileName = "PacketParse.txt";
        private static List<string> lines = new();

        public static void Main(string[] args)
        {
            TextManager.Instance.Initialise();
            
            MessageManager.Instance.Initialise();
            TunnelDataManager.Instance.Initialise();
            AuthMessageManager.Instance.Initialise();
            GatewayMessageManager.Instance.Initialise();
            ZoneMessageManager.Instance.Initialise();

            authServerStream = new DataStreamInput(null);
            authServerStream.OnData += (gamePacket) =>
            {
                OnGamePacket(gamePacket, GamePacketType.Auth, false);
            };
            authClientStream = new DataStreamInput(null);
            authClientStream.OnData += (gamePacket) =>
            {
                OnGamePacket(gamePacket, GamePacketType.Auth, true);
            };

            zoneServerStream = new DataStreamInput(null);
            zoneServerStream.OnData += (gamePacket) =>
            {
                OnGamePacket(gamePacket, GamePacketType.Gateway, false);
            };
            zoneServerStream.SetEncryption(false);
            zoneClientStream = new DataStreamInput(null);
            zoneClientStream.OnData += (gamePacket) =>
            {
                OnGamePacket(gamePacket, GamePacketType.Gateway, true);
            };
            zoneClientStream.SetEncryption(false);

            OpenPcapORPcapNGFile($"Auth.pcapng", CancellationToken.None);
            File.WriteAllLines(FileName, lines.ToArray());
        }

        private static void OnGamePacket(byte[] data, GamePacketType gamePacketType, bool isClient)
        {
            string header = isClient ? ClientHeader : ServerHeader;
            byte[] opcode = gamePacketType == GamePacketType.Auth ? new byte[1] { data[0] } : new byte[2] { data[0], data[1] };

            switch (gamePacketType)
            {
                case GamePacketType.Auth:
                    HandleAuthMessage(data, isClient);
                    break;
                case GamePacketType.Gateway:
                    if (!zoneClientStream.UsingEncryption)
                        zoneClientStream.SetEncryption(true);
                    if (!zoneServerStream.UsingEncryption)
                        zoneServerStream.SetEncryption(true);
                    HandleGatewayPacket(data, isClient);
                    break;
                case GamePacketType.Zone:
                    (ZoneMessageOpcode? zoneOpcode, int offset) = WorldSession.GetOpcode(data);

                    if (zoneOpcode == null)
                    {
                        log.Warn($"Unknown Zone Packet : {BitConverter.ToString(data)}");
                        return;
                    }

                    var newData = new Span<byte>(data, offset, data.Length - offset).ToArray();
                    lines.Add($"{header} [Zone] {zoneOpcode} (0x{(int)zoneOpcode:X8}) | {newData.Length} bytes");
                    if (zoneOpcode == ZoneMessageOpcode.SendSelfToClient)
                        File.WriteAllText("SendSelf.bin", BitConverter.ToString(data));
                    else
                        lines.Add($"{BitConverter.ToString(data)}");

                    var packet = new ZonePacket((ZoneMessageOpcode)zoneOpcode, data);
                    break;
            }

            File.WriteAllLines(FileName, lines.ToArray());
        }

        private static void HandleGatewayPacket(byte[] data, bool isClient)
        {
            string header = isClient ? ClientHeader : ServerHeader;
            var opcode = (GatewayMessageOpcode)(data[0] & 0x1F);
            data = new Span<byte>(data, 1, data.Length - 1).ToArray();

            //lines.Add($"{header} [Gateway] {opcode} (0x{(int)opcode:X8})");
            log.Info($"{header} {opcode} (0x{(int)opcode:X8}) : {BitConverter.ToString(data)}");

            // Handle Tunnel Packets slightly separately.
            if (opcode == GatewayMessageOpcode.TunnelPacketFromExternalConnection || opcode == GatewayMessageOpcode.TunnelPacketToExternalConnection)
            {
                OnGamePacket(data, GamePacketType.Zone, isClient);
                return;
            }

            IReadable message = GatewayMessageManager.Instance.GetGatewayMessage(opcode, ProtocolVersion.ExternalGatewayApi_3);
            if (message == null)
            {
                log.Warn($"Received unknown Gateway packet {opcode:X} : {BitConverter.ToString(data)}");
                return;
            }

            //log.Debug($"Received packet {packet.Opcode}(0x{packet.Opcode:X})  : {BitConverter.ToString(packet.Data)}");

            var reader = new GamePacketReader(data);
            message.Read(reader);

            lines.Add(JsonConvert.SerializeObject(message, Formatting.Indented));
        }

        private static void HandleAuthMessage(byte[] data, bool isClient)
        {
            string header = isClient ? ClientHeader : ServerHeader;
            var opcode = (AuthMessageOpcode)data[0];

            //log.Info($"{header} {opcode} (0x{data[0]:X8}) : {BitConverter.ToString(data)}");
            lines.Add($"{header} [Auth] {opcode} (0x{data[0]:X8})");
            //lines.Add($"{BitConverter.ToString(data)}");
            IReadable message = AuthMessageManager.Instance.GetAuthMessage(opcode, ProtocolVersion.LOGIN_ALL);
            if (message == null)
            {
                log.Warn($"{header} Received unknown auth packet {opcode:X} : {BitConverter.ToString(data)}");
                return;
            }

            data = new Span<byte>(data, 1, data.Length - 1).ToArray();
            var reader = new GamePacketReader(data);
            message.Read(reader);

            if (message is CharacterLoginReply loginReply)
            {
                zoneClientStream.SetEncryptionKey(Convert.ToBase64String(loginReply.Server.EncryptionKey));
                zoneServerStream.SetEncryptionKey(Convert.ToBase64String(loginReply.Server.EncryptionKey));
            }

            lines.Add(JsonConvert.SerializeObject(message, Formatting.Indented));
        }

        private static void OpenPcapORPcapNGFile(string filename, CancellationToken token)
        {
            using (var reader = IReaderFactory.GetReader(filename))
            {
                reader.OnReadPacketEvent += reader_OnReadPacketEvent;
                reader.ReadPackets(token);
                reader.OnReadPacketEvent -= reader_OnReadPacketEvent;
            }
        }

        private static void reader_OnReadPacketEvent(object context, IPacket packet)
        {

            byte[] ipHeaderData = new Span<byte>(packet.Data, 14, packet.Data.Length - 14).ToArray();
            var ipHeader = new IpHeader(ipHeaderData, ipHeaderData.Length);
            if (ipHeader.ProtocolType != Protocol.UDP)
                return;
            
            GetGamePacketType(packet, out GamePacketType gamePacketType, out UdpHeader udpHeader);
            bool isClient = udpHeader.SourcePort > 40000;

            byte[] data = new Span<byte>(packet.Data, 42, packet.Data.Length - 42).ToArray();
            var protoPacket = new ProtocolPacket(data, new ParserPacketOptions
            {
                IsSubpacket = false,
                Compression = true,
                GamePacketType = gamePacketType,
                IsClient = isClient
            });
            HandleProtocolPacket(protoPacket);
        }

        private static void GetGamePacketType(IPacket packet, out GamePacketType gamePacketType, out UdpHeader udpHeader)
        {
            byte[] ports = new Span<byte>(packet.Data, 34, 4).ToArray();
            udpHeader = new UdpHeader(ports, ports.Length);
            if (udpHeader.SourcePort == 20042 || udpHeader.DestinationPort == 20042)
                gamePacketType = GamePacketType.Auth;
            else
                gamePacketType = GamePacketType.Gateway;
        }

        private static void HandleProtocolPacket(ProtocolPacket packet)
        {
            var packetOptions = packet.PacketOptions as ParserPacketOptions;
            string header = packetOptions.IsClient ? ClientHeader : ServerHeader;

            //if (packetOptions.IsClient && packetOptions.GamePacketType == GamePacketType.Zone)
            //    return;

            IProtocol message = MessageManager.Instance.GetProtocolMessage(packet.Opcode);
            if (message == null)
            {
                log.Warn($"{header} Received unknown protocol packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
                return;
            }

            if (message is OutOfOrder)
                return;

            if (packetOptions.GamePacketType == GamePacketType.Gateway)
                packet.PacketOptions.Compression = false;

            log.Trace($"{header} Received protocol packet {packet.Opcode:X} : {BitConverter.ToString(packet.Data)}");
            var reader = new ProtocolPacketReader(packet.Data);
            message.Read(reader, packet.PacketOptions);

            if (message is SessionRequest sessionRequest)
            {
                lines.Add($"{header} [Protocol] {packet.Opcode} (0x{(int)(packet.Opcode):X8})");
                lines.Add(JsonConvert.SerializeObject(sessionRequest, Formatting.Indented));
            }

            if (message is SessionReply sessionReply)
            {
                lines.Add($"{header} [Protocol] {packet.Opcode} (0x{(int)(packet.Opcode):X8})");
                lines.Add(JsonConvert.SerializeObject(sessionReply, Formatting.Indented));
            }

            if (message is MultiPacket multiPacket)
            {
                foreach (ProtocolPacket multiPacketPacket in multiPacket.Packets)
                {
                    multiPacketPacket.PacketOptions = new ParserPacketOptions
                    {
                        IsSubpacket = true,
                        Compression = packetOptions.GamePacketType != GamePacketType.Gateway,
                        GamePacketType = packetOptions.GamePacketType,
                        IsClient = packetOptions.IsClient
                    };
                    HandleProtocolPacket(multiPacketPacket);
                }
            }

            if (message is DataWhole dataWhole)
            {
                if (packetOptions.GamePacketType == GamePacketType.Auth)
                {
                    if (packetOptions.IsClient)
                        authClientStream.ProcessDataFragment(dataWhole);
                    else
                        authServerStream.ProcessDataFragment(dataWhole);
                }
                else if (packetOptions.GamePacketType == GamePacketType.Gateway)
                {
                    if (packetOptions.IsClient)
                        zoneClientStream.ProcessDataFragment(dataWhole);
                    else
                        zoneServerStream.ProcessDataFragment(dataWhole);
                }
            }
            
            if (message is DataFragment fragment)
            {
                if (packetOptions.GamePacketType == GamePacketType.Auth)
                {
                    if (packetOptions.IsClient)
                        authClientStream.ProcessDataFragment(fragment);
                    else
                        authServerStream.ProcessDataFragment(fragment);
                }
                else if (packetOptions.GamePacketType == GamePacketType.Gateway)
                {
                    if (packetOptions.IsClient)
                        zoneClientStream.ProcessDataFragment(fragment);
                    else
                        zoneServerStream.ProcessDataFragment(fragment);
                }
            }
        }
    }
}
