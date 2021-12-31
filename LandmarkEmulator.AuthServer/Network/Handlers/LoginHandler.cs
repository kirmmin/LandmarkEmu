using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Network.Message.Model;
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
                Status   = 1
            });

            session.EnqueueMessage(new ServerListReply
            {
                Servers = new System.Collections.Generic.List<ServerListReply.Server>
                {
                    new ServerListReply.Server
                    {
                        ServerId    = 0x100,
                        IsLocked    = false,
                        Name        = "SoloServer",
                        NameId      = 101,
                        Description = "yeah",
                        ServerInfo  = "<ServerInfo Region=\"CharacterCreate.RegionUs\" Subregion=\"UI.SubregionUSEast\" IsRecommended=\"1\"></ServerInfo>",
                        PopulationLevel = 3, // Ignored by Server
                        PopulationData = "<Population ServerPlayerCapacity=\"75\" ServerClaimCapacity=\"0\" Claims=\"0\" IsRecommended=\"0\" FriendsOnline=\"3\"></Population>"
                    }
                }
            });

            session.EnqueueMessage(new CharacterSelectInfoReply
            {
                Status = 1,
                CanBypassServerLock = true
            });
        }
    }
}
