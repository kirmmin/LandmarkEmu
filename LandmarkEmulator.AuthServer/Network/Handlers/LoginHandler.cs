using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Network.Message.Model;
using LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData;
using LandmarkEmulator.AuthServer.Zone;
using LandmarkEmulator.Shared.Game.Entity.Static;
using LandmarkEmulator.Shared.Network.Message;
using NLog;
using System.Collections.Generic;

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
                LoggedIn   = true,
                Status     = 1,
                IsMember   = false, // Must be false if ProtocolVersion 9
                IsInternal = false  // Must be false if ProtocolVersion 9
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.ServerListRequest)]
        public static void HandleServerListRequest(AuthSession session, ServerListRequest request)
        {
            var serverListReply = new ServerListReply();
            
            foreach (var zoneServer in ZoneServerManager.Instance.ZoneServers)
                serverListReply.Servers.Add(zoneServer.Build());

            session.EnqueueMessage(serverListReply);
        }

        [AuthMessageHandler(AuthMessageOpcode.CharacterSelectInfoRequest)]
        public static void HandleCharacterSelectInfoRequest(AuthSession session, CharacterSelectInfoRequest request)
        {
            session.EnqueueMessage(new CharacterSelectInfoReply
            {
                Status = 1,
                CanBypassServerLock = true,
                Characters = new List<CharacterSelectInfoReply.Character>
                {
                    new CharacterSelectInfoReply.Character
                    {
                        CharacterId = 1ul,
                        LastServerId = 0x100,
                        LastLogin = 1d,
                        Status = 1u,
                        CharacterData = new CharacterSelectInfoReply.Character.CharacterPayload
                        {
                            CharacterAttachments = new List<CharacterSelectInfoReply.Character.CharacterPayload.CharacterAttachment>
                            {
                                // TODO: Figure out Animation/BaseModel field. Female models seem contorted to Male stance.
                                new CharacterSelectInfoReply.Character.CharacterPayload.CharacterAttachment
                                {
                                    ModelName = "Char_Biped_HumanFemale_Entities_Townsperson_000_Chest.adr",
                                    Slot = AttachmentSlot.ChestModel
                                },
                                new CharacterSelectInfoReply.Character.CharacterPayload.CharacterAttachment
                                {
                                    ModelName = "Char_Biped_DarkElfMale_Entities_PCNPC_DarkElf_Light_Chest.adr",
                                    //ModelName = "Char_Biped_HumanFemale_Entities_Gunslinger_Medium_001_Chest.adr",
                                    Slot = AttachmentSlot.ChestVisual
                                }
                            }
                        }
                    }
                }
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.TunnelPacketClientToServer)]
        public static void HandleTunnelPacketClientToServer(AuthSession session, TunnelPacketClientToServer packet)
        {
            switch (packet.Data)
            {
                case NameValidationRequest nameValidationRequest:
                    log.Trace($"{nameValidationRequest.FirstName}, {nameValidationRequest.LastName}");

                    // TODO: Actually confirm name is available.
                    session.EnqueueMessage(new TunnelPacketServerToClient
                    {
                        Type     = Message.Static.TunnelDataType.NameValidationReply,
                        Data     = new NameValidationReply
                        {
                            Result    = Message.Static.NameValidationResult.Success,
                            FirstName = nameValidationRequest.FirstName
                        }
                    });
                    break;
                case LoginInit loginInit:
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

                    // TODO: Figure out the ArtData packet for Tints
                    session.EnqueueMessage(new TunnelPacketServerToClient
                    {
                        ServerId = 0,
                        Type = Message.Static.TunnelDataType.ArtData,
                        Data = new ArtData
                        {
                            Unknown0 = new System.Collections.Generic.List<ArtData.UnknownStruct_142C36820>
                            {
                                new ArtData.UnknownStruct_142C36820
                                {
                                    Unknown0 = 1121,
                                    Unknown1 = "Human_Skin_000"
                                }
                            },
                            Unknown1 = new System.Collections.Generic.List<ArtData.UnknownStruct_142C351D0>
                            {
                                new ArtData.UnknownStruct_142C351D0
                                {
                                    Unknown0 = 1,
                                    Unknown1 = 2
                                }
                            },
                            ArtTintOverrideGroups = new System.Collections.Generic.List<ArtData.ArtTintOverrideGroupEntry>
                            {
                                new ArtData.ArtTintOverrideGroupEntry
                                {
                                    Unknown0 = 3,
                                    Unknown1 = 4,
                                    Unknown2 = 5,
                                    Unknown3 = new System.Collections.Generic.List<ArtData.ArtTintOverrideGroupEntry.ArtTintOverrideEntry>
                                    {
                                        new ArtData.ArtTintOverrideGroupEntry.ArtTintOverrideEntry
                                        {
                                            Unknown0 = 6,
                                            Unknown1 = 7
                                        }
                                    }
                                }
                            },
                            Unknown3 = new System.Collections.Generic.List<ArtData.UnknownStruct_142C3FE00>
                            {
                                new ArtData.UnknownStruct_142C3FE00
                                {
                                    Unknown0 = 8,
                                    Unknown1 = 9,
                                    Unknown2 = 10,
                                    Unknown3 = 11,
                                    Unknown4 = 12
                                },
                                new ArtData.UnknownStruct_142C3FE00
                                {
                                    Unknown0 = 13,
                                    Unknown1 = 14,
                                    Unknown2 = 15,
                                    Unknown3 = 16,
                                    Unknown4 = 17
                                }
                            },
                            Unknown4 = new System.Collections.Generic.List<ArtData.UnknownStruct_142C3BAC0>
                            {
                                new ArtData.UnknownStruct_142C3BAC0
                                {
                                    Unknown0 = "Human_Skin_000"
                                }
                            },
                            Unknown5 = new System.Collections.Generic.List<ArtData.UnknownStruct_142C3BAC0>
                            {
                                new ArtData.UnknownStruct_142C3BAC0
                                {
                                    Unknown0 = "Human_Skin_000"
                                }
                            },
                            ArtTintGroups = new System.Collections.Generic.List<ArtData.TintSemanticGroupEntry>
                            {
                                new ArtData.TintSemanticGroupEntry
                                {
                                    Id            = 0,
                                    AliasName     = "BaseTint",
                                    SemanticGroup = "ANY",
                                    ArtTints      = new System.Collections.Generic.List<ArtData.TintSemanticGroupEntry.TintSemanticEntry>
                                    {
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintSmoothnessA",
                                            EditType     = "RGB",
                                            Unknown1     = 607,
                                            R            = 1f,
                                            G            = 0.96862745098f,
                                            B            = 0.882352941176f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintDielectricA",
                                            EditType     = "RGB",
                                            Unknown1     = 607,
                                            R            = 0.666666666667f,
                                            G            = 0.686274509804f,
                                            B            = 0.792156862745f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintMetallicA",
                                            EditType     = "RGB",
                                            Unknown1     = 607,
                                            R            = 0.117647058824f,
                                            G            = 0.109803921569f,
                                            B            = 0.0549019607843f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintSmoothnessA",
                                            EditType     = "SCALAR",
                                            Unknown1     = 607,
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintDielectricA",
                                            EditType     = "SCALAR",
                                            Unknown1     = 607,
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintMetallicA",
                                            EditType     = "SCALAR",
                                            Unknown1     = 607,
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintSmoothnessB",
                                            EditType     = "SCALAR",
                                            Unknown1     = 607,
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintDielectricB",
                                            EditType     = "SCALAR",
                                            Unknown1     = 607,
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        },
                                        new ArtData.TintSemanticGroupEntry.TintSemanticEntry
                                        {
                                            SemanticName = "BaseTintMetallicB",
                                            EditType     = "SCALAR",
                                            Unknown1     = 607,
                                            R            = 0.5f,
                                            G            = 0f,
                                            B            = 0f
                                        }
                                    }
                                }
                            },
                            Unknown7 = new System.Collections.Generic.List<ArtData.UnknownStruct_142C351D0>
                            {
                                new ArtData.UnknownStruct_142C351D0
                                {
                                    Unknown0 = 1121,
                                    Unknown1 = 1121
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

        [AuthMessageHandler(AuthMessageOpcode.CharacterCreateRequest, ProtocolVersion.LoginUdp_10)]
        public static void HandleCharacterCreateRequest(AuthSession session, CharacterCreateRequest request)
        {
            session.EnqueueMessage(new CharacterCreateReply
            {
                Result      = Message.Static.CharacterCreateResult.Success,
                CharacterId = 1
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.CharacterCreateRequest, ProtocolVersion.LoginUdp_9)]
        public static void HandleCharacterCreateRequest9(AuthSession session, CharacterCreateRequest9 request)
        {
            session.EnqueueMessage(new CharacterCreateReply
            {
                Result = Message.Static.CharacterCreateResult.UnableToCreate,
                CharacterId = 1
            });
        }

        [AuthMessageHandler(AuthMessageOpcode.CharacterDeleteRequest)]
        public static void HandleCharacterDeleteRequest(AuthSession session, CharacterDeleteRequest request)
        {
            session.EnqueueMessage(new CharacterSelectInfoReply
            {
                Status = 1,
                CanBypassServerLock = true,
                Characters = new List<CharacterSelectInfoReply.Character>
                {
                }
            });
            session.EnqueueMessage(new CharacterDeleteReply
            {
                CharacterId = request.CharacterId,
                Status      = 1
            });
        }
    }
}
