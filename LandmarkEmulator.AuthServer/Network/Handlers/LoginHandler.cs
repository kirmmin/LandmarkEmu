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
                        Data = new ArtData
                        {
                            ArtTintGroups = new System.Collections.Generic.List<ArtData.TintSemanticGroupEntry>
                            {
                                new ArtData.TintSemanticGroupEntry
                                {
                                    Id            = 1121,
                                    AliasName     = "Human_Skin_000",
                                    SemanticGroup = "BaseTint",
                                    ArtTints      = new System.Collections.Generic.List<ArtData.TintSemanticGroupEntry.TintSemanticEntry>
                                    {
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintSmoothnessA",
                                            EditType     = "RGB",
                                            R            = 1f,
                                            G            = 0.96862745098f,
                                            B            = 0.882352941176f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintDielectricA",
                                            EditType     = "RGB",
                                            R            = 0.666666666667f,
                                            G            = 0.686274509804f,
                                            B            = 0.792156862745f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintMetallicA",
                                            EditType     = "RGB",
                                            R            = 0.117647058824f,
                                            G            = 0.109803921569f,
                                            B            = 0.0549019607843f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintSmoothnessA",
                                            EditType     = "SCALAR",
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintDielectricA",
                                            EditType     = "SCALAR",
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintMetallicA",
                                            EditType     = "SCALAR",
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintSmoothnessB",
                                            EditType     = "SCALAR",
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintDielectricB",
                                            EditType     = "SCALAR",
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintMetallicB",
                                            EditType     = "SCALAR",
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        }
                                    }
                                }
                            }
                        }
                    });
                    break;
                default:
                    log.Warn($"Unknown Tunnel Data");
                    break;
            }
        }
    }
}
