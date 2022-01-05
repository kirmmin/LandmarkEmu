using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Network.Message.Model;
using LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData;
using NLog;

namespace LandmarkEmulator.AuthServer.Network.Handlers
{
    public static class LoginHandler
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        [AuthMessageHandler(AuthMessageOpcode.LoginRequest)]
        public static void HandleLoginRequest(AuthSession session, LoginRequest request)
        {
            log.Info($"{request.SessionId}, {request.Locale}, {request.ThirdPartyAuthTicket}");
            log.Info($"{request.SystemFingerPrint}");

            session.EnqueueMessage(new LoginReply
            {
                LoggedIn = true,
                Status   = 1,
                Result   = 1
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.ServerListRequest)]
        public static void HandleServerListRequest(AuthSession session, ServerListRequest request)
        {
            session.EnqueueMessage(new ServerListReply
            {
                Servers = new System.Collections.Generic.List<ServerListReply.Server>
                {
                    new ServerListReply.Server
                    {
                        ServerId    = 0x100,
                        AllowedAccess = true,
                        IsLocked    = false,
                        Name        = "SoloServer",
                        Description = "yeah",
                        ServerInfo  = "<ServerInfo Region=\"CharacterCreate.RegionUs\" Subregion=\"UI.SubregionUSEast\" IsRecommended=\"1\"></ServerInfo>",
                        PopulationLevel = 3, // Ignored by Server
                        PopulationData = "<Population ServerPlayerCapacity=\"75\" ServerClaimCapacity=\"50\" Claims=\"0\" IsRecommended=\"0\" FriendsOnline=\"3\"></Population>"
                    }
                }
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.CharacterSelectInfoRequest)]
        public static void HandleCharacterSelectInfoRequest(AuthSession session, CharacterSelectInfoRequest request)
        {
            session.EnqueueMessage(new CharacterSelectInfoReply
            {
                Status = 1,
                CanBypassServerLock = true
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.TunnelPacketClientToServer)]
        public static void HandleTunnelPacketClientToServer(AuthSession session, TunnelPacketClientToServer packet)
        {
            log.Info($"{packet.Type}");

            switch (packet.Data)
            {
                case LoginInit unknown6:
                    log.Trace($"{unknown6}");
                    session.EnqueueMessage(new TunnelPacketServerToClient
                    {
                        ServerId = 0,
                        Type = Message.Static.TunnelDataType.Unknown7,
                        Data = new Unknown7
                        {
                            String = "Welcome to Landmark RE:Build! We're happy to have you!"
                        }
                    });
                    session.EnqueueMessage(new TunnelPacketServerToClient
                    {
                        ServerId = 0,
                        Type = Message.Static.TunnelDataType.ClaimData,
                        Data = new ClaimData()
                    });
                    session.EnqueueMessage(new TunnelPacketServerToClient
                    {
                        ServerId = 0,
                        Type = Message.Static.TunnelDataType.ArtData,
                        Data = new ArtData()
                    });
                    break;
                default:
                    log.Warn($"Unknown Tunnel Data");
                    break;
            }
        }
    }
}
