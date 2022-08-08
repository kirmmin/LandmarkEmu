﻿namespace LandmarkEmulator.WorldServer.Network.Message
{
    public enum ZoneMessageOpcode : uint
    {
        ZoneLoginRequest            = 0x400,
        ZoneLoginReply              = 0x500,
        ZoneLogout                  = 0x600,
        ZoneIsReady                 = 0x700,
        ZoneStats                   = 0x800,
        TransferPlayerToServer      = 0x900,
        SavePlayer                  = 0xA00,
        PlayerTransferServerRequest = 0xB00,
        PlayerTransferServerReply   = 0xC00,
        ClientFinishedLoading       = 0xD00,
        EncounterStatus             = 0xE00,
        SendSelfToClient            = 0xF00,
        ClientIsReady               = 0x1000,
        ZoneDoneSendingInitialData  = 0x1100,
        ZoneCoordinateInfo          = 0x1200,
        ChatBase                    = 0x1300,
        ClientLogout                = 0x1400,
        ProxiedPlayerBase           = 0x1500,
        WorldInitiatedPlayerTransferServer = 0x1600,
        ZoneToWorldForPlayer        = 0x1700,
        ZoneToZonePacket            = 0x1800,
        ClientToTargetClientsZoneWrapped = 0x1900,
        TargetClientNotOnline       = 0x1A00,
        WorldToClientWrapped        = 0x1B00,
        WorldToZoneForPlayer        = 0x1C00,
        PcProfileActivated          = 0x1D00,
        CommandBase                 = 0x1E00,
        AdminBase                   = 0x1F00,
        LogoutPlayer                = 0x2000,
        ShutdownZone                = 0x2100,
        ZoneReportingClientIsReady  = 0x2200,
        ClientBeginZoning           = 0x2300,
        CombatBase                  = 0x2400,
        VehicleDemolitionDerbyBase  = 0x2500,
        PlayerUpdateBase            = 0x2600,
        AbilityBase                 = 0x2700,
        SavePlayerTrackedStats      = 0x2800,
        ClientUpdateHitpoints       = 0x29000100,
        ClientUpdateItemAdd         = 0x29000200,
        ClientUpdateItemUpdate      = 0x29000300,
        ClientUpdateItemDelete      = 0x29000400,
        ClientUpdateItemDeleteFailed = 0x29000500,
        ClientUpdateUpdateStat      = 0x29000600,
        ClientUpdateCollectionStart = 0x29000700,
        ClientUpdateCollectionRemove = 0x29000800,
        ClientUpdateCollectionAddEntry = 0x29000900,
        ClientUpdateCollectionRemoveEntry = 0x29000A00,
        ClientUpdateUpdateLocation  = 0x29000B00,
        ClientUpdateTeleportToLocation = 0x29000C00,
        ClientUpdateMana            = 0x29000D00,
        ClientUpdateUpdateProfileExperience = 0x29000E00,
        ClientUpdateAddProfileAbilitySetApl = 0x2900F00,
        ClientUpdateAddEffectTag    = 0x29001000,
        ClientUpdateRemoveEffectTag = 0x29001100,
        ClientUpdateUpdateProfileRank = 0x29001200,
        ClientUpdateCoinCount       = 0x29001300,
        ClientUpdateDeleteProfile = 0x29001400,
        ClientUpdateActivateProfile = 0x29001500,
        ClientUpdateAddAbility      = 0x29001600,
        ClientUpdateNotifyPlayer    = 0x29001700,
        ClientUpdateUpdateProfileAbilitySetApl = 0x29001800,
        ClientUpdateRemoveActionBars = 0x29001900,
        ClientUpdateUpdateActionBarSlot = 0x29001A00,
        ClientUpdateDoneSendingPreloadCharacters = 0x29001B00,
        ClientUpdateUpdateActionBarSlotUsed = 0x29001C00,
        ClientUpdatePhaseChange     = 0x29001D00,
        ClientUpdateDamageInfo      = 0x29001E00,
        ClientUpdateZonePopulation  = 0x29001F00,
        ClientUpdateRespawnLocations = 0x29002000,
        ClientUpdateModifyMovementSpeed = 0x29002100,
        ClientUpdateModifyTurnRate  = 0x29002200,
        ClientUpdateModifyStrafeSpeed = 0x29002300,
        ClientUpdateUpdateManagedLocation = 0x29002400,
        ClientUpdatePacketScreenEffect = 0x29002500,
        ClientUpdateMovementVersion = 0x29002600,
        ClientUpdateManagedMovementVersion = 0x29002700,
        ClientUpdateUpdateWeaponAddClips = 0x29002800,
        ClientUpdateVaultItem       = 0x29002900,
        ClientUpdateContainerItem   = 0x29002A00,
        ClientUpdateLoyaltyPoints   = 0x29002B00,
        ClientUpdateUpdateCalculatedStats = 0x29002C00,
        ClientUpdateCollectionItem  = 0x29002D00,
        MiniGameBase                = 0x2A00,
        GroupsBase                  = 0x2B00,
        EncounterBase               = 0x2C00,
        InventoryBase               = 0x2D00,
        SendZoneDetails             = 0x2E00,
        ReferenceDataBase           = 0x2F00,
        ObjectiveBase               = 0x3000,
        DebugBase                   = 0x3100,
        UiBase                      = 0x3200,
        ImmediatelyDeletePlayerFromZone = 0x3300,
        QuestBase                   = 0x3400,
        RewardBase                  = 0x3500,
        EncounterInfo               = 0x3600,
        GameTimeSync                = 0x3700,
        PetBase                     = 0x3900,
        ZoneQuestStatus             = 0x3A00,
        PlayerLocation              = 0x3B00,
        PointOfInterestDefinitionRequest = 0x3C00,
        PointOfInterestDefinitionReply = 0x3D00,
        TradeBase                   = 0x3E00,
        EscrowGivePackage           = 0x3F00,
        EscrowGotPackage            = 0x4000,
        UpdateEncounterDataCommon   = 0x4100,
        RecipeBase                  = 0x4200,
        ZoneWorldMiniGameStatus     = 0x4300,
        ZoneWorldNpcKnockout        = 0x4400,
        InGamePurchaseBase          = 0x4500,
        QuickChatBase               = 0x4600,
        ReportBase                  = 0x4700,
        LiveGamerBase               = 0x4800,
        WorldZoneRenamePlayerRequest = 0x4900,
        WorldZoneRenamePlayerReply  = 0x4A00,
        AcquaintanceBase            = 0x4B00,
        ClientServerShuttingDown    = 0x4C00,
        FriendBase                  = 0x4D00,
        WorldZoneSyncTime           = 0x4E00,
        BroadcastBase               = 0x4F00,
        NpcTransferServerRequest    = 0x5000,
        NpcTransferServerReply      = 0x5100,
        NpcTransferServer           = 0x5200,
        ClientKickedFromServer      = 0x5300,
        UpdateClientSessionData     = 0x5400,
        BugSubmissionBase           = 0x5500,
        EncounterPlayerLeft         = 0x5700,
        EncounterKickPlayer         = 0x5800,
        WorldDisplayInfo            = 0x5900,
        MOTD                        = 0x5A00,
        SetLocale                   = 0x5B00,
        SetClientArea               = 0x5C00,
        ZoneTeleportRequest         = 0x5D00,
        TradingCardBase             = 0x5E00,
        WorldShutdownNotice         = 0x5F00,
        LoadWelcomeScreen           = 0x6000,
        ShipCombatBase              = 0x6100,
        AdminMiniGameBase           = 0x6200,
        KeepAlive                   = 0x6300,
        ClientExitLaunchUri         = 0x6400,
        ClientPendingKickFromServer = 0x6500,
        ClientMembershipActivation  = 0x6600,
        LobbyBase                   = 0x6700,
        LobbyGameDefinitionBase     = 0x6800,
        ShowSystemMessage           = 0x6900,
        POIChangeMessage            = 0x6A00,
        ClientMetrics               = 0x6B00,
        ZoneWorldPlayerCompletedTutorial = 0x6C00,
        FirstTimeEventBase          = 0x6D00,
        RequisitionBase             = 0x6E00,
        ClientLog                   = 0x6F00,
        LockZoningInRequest         = 0x7000,
        LockZoningInReply           = 0x7100,
        IgnoreBase                  = 0x7200,
        SnoopedPlayerBase           = 0x7300,
        PromotionalBase             = 0x7400,
        AddClientPortraitCrc        = 0x7500,
        ObjectiveTargetBase         = 0x7600,
        CommerceSessionRequest      = 0x7700,
        CommerceSessionResponse     = 0x7800,
        TrackedEvent                = 0x7900,
        ClientLoginFailed           = 0x7A00,
        LoginToUChat                = 0x7B00,
        ZoneSafeTeleportRequest     = 0x7C00,
        RemoteInteractionRequest    = 0x7D00,
        PlayerOutsideZoneBounds     = 0x7E00,
        PlayerUpdatePosition        = 0x7F00,
        PlayerUpdateCamera          = 0x8000,
        HousingBase                 = 0x8100,
        StaticZoneDisconnected      = 0x8200,
        GuildBase                   = 0x8300,
        AdminGuildBase              = 0x8400,
        BattleMagesBase             = 0x8500,
        ValidateDataForZoneOwnedTiles = 0x8600,
        ValidateSpawnLocations      = 0x8700,
        WorldToWorldBase            = 0x8800,
        PerformAction               = 0x8900,
        EncounterMatchmakingBase    = 0x8A00,
        ClientLuaMetrics            = 0x8B00,
        UpdateClientCountry         = 0x8C00,
        RepeatingActivityBase       = 0x8D00,
        ClientGameSettings          = 0x8E00,
        ActivityManagerBase         = 0x8F00,
        RequestSendItemDefinitionsToClient = 0x9000,
        InspectBase                 = 0x9100,
        AchievementBase             = 0x9200,
        ExternalVendorFulfillmentRequest = 0x9300,
        PlayerTitleBase             = 0x9400,
        UpdatePlayerSession         = 0x9500,
        UpdateStationAccountProperties = 0x9600,
        JobCustimzationBase         = 0x9700,
        FotomatBase                 = 0x9800,
        LootBase                    = 0x9900,
        ActionManagerBase           = 0x9A00,
        AdminSocialProfileBase      = 0x9B00,
        SocialProfileBase           = 0x9C00,
        PlayerUpdateJump            = 0x9D00,
        CoinStoreBase               = 0x9E00,
        InitializationParameters    = 0x9F00,
        ActivityBase                = 0xA000,
        MountBase                   = 0xA100,
        ClientInitializationDetails = 0xA200,
        UpdateStartingWalletBalance = 0xA300,
        ClientNotifyCoinSpinAvailable = 0xA400,
        ClientAreaTimer             = 0xA500,
        LoyaltyRewardBase           = 0xA600,
        RatingBase                  = 0xA700,
        ClientActivityLaunchBase    = 0xA800,
        ServerActivityLaunchBase    = 0xA900,
        ClientFlashTimer            = 0xAA00,
        UpdatePlayerFriendStatus    = 0xAB00,
        MiniGameChallengeResult     = 0xAC00,
        InviteAndStartMiniGame      = 0xAD00,
        PlayerUpdateFlourish        = 0xAE00,
        QuizBase                    = 0xAF00,
        ReplyGuidBank               = 0xB000,
        RequestGuidBank             = 0xB100,
        PlayerUpdatePositionOnPlatform = 0xB200,
        ClientMembershipVipInfo     = 0xB300,
        ZoneMembershipVipInfo       = 0xB400,
        TargetBase                  = 0xB500,
        ComboBase                   = 0xB600,
        GuideStoneBase              = 0xB700,
        RaidsBase                   = 0xB800,
        VoiceBase                   = 0xB900,
        WeaponBase                  = 0xBA00,
        PunkBuster                  = 0xBB00,
        SkillsBase                  = 0xBC00,
        LoadoutsBase                = 0xBD00,
        ExperienceBase              = 0xBE00,
        VehicleBase                 = 0xBF00,
        GriefBase                   = 0xC000,
        SpotPlayer                  = 0xC100,
        FactionsBase                = 0xC200,
        Synchronization             = 0xC300,
        ResourcesBase               = 0xC400,
        CollisionBase               = 0xC500,
        LeaderboardBase             = 0xC600,
        PlayerUpdateManagedPosition = 0xC700,
        UpdatePlayerUnderage        = 0xC800,
        PlayerUpdateVehicleWeapon   = 0xC900,
        ProfileStatsBase            = 0xCA00,
        EquipmentBase               = 0xCB00,
        DefinitionFiltersBase       = 0xCC00,
        GetRespawnLocations         = 0xCD00,
        WallOfDataBase              = 0xCE00,
        ThrustPadBase               = 0xCF00,
        InGamePurchase              = 0xD000,
        MissionsBase                = 0xD100,
        EffectsBase                 = 0xD200,
        RewardBuffsBase             = 0xD300,
        AbilitiesBase               = 0xD400,
        DeployableBase              = 0xD500,
        Security                    = 0xD600,
        MapRegionBase               = 0xD700,
        SelectiveBroadcast          = 0xD800,
        UniversalSetZoneVars        = 0xD900,
        JoinChannel                 = 0xDA00,
        LeaveChannel                = 0xDB00,
        HudManagerBase              = 0xDC00,
        ClientPcDataBase            = 0xDD00,
        AcquireTimersBase           = 0xDE00,
        UpdateGuildTag              = 0xDF00,
        LoginQueueStatus            = 0xE000,
        ServerPopulationInfo        = 0xE100,
        GetServerPopulationInfo     = 0xE200,
        PlayerUpdateVehicleCollision = 0xE300,
        PlayerStop                  = 0xE400,
        CurrencyBase                = 0xE500,
        ItemsBase                   = 0xE600,
        PlayerUpdateAttachObject    = 0xE700,
        PlayerUpdateDetachObject    = 0xE800,
        ClientSettings              = 0xE900,
        AreaDefinitionBase          = 0xEA00,
        EnvironmentBase             = 0xEB00,
        TerrainCellClientCellRequest = 0xEC000100,
        TerrainCellPendingRequestTimeout = 0xEC000200,
        TerrainCellClientCellUpdateResponse = 0xEC000300,
        TerrainCellUpToDateCells    = 0xEC000400,
        TerrainCellCellsUpdate      = 0xEC000500,
        TerrainCellMaterialChangeInfo = 0xEC000600,
        TerrainCellCellBlockDamage  = 0xEC000700,
        TerrainCellResetAllCells    = 0xEC000800,
        TerrainCellResetTemporaryCells = 0xEC000900,
        TerrainCellModifiedCellManifest = 0xEC000A00,
        TerrainCellModifiedCellManifestUpdate = 0xEC000B00,
        TerrainCellZoneProcessingComplete = 0xEC000C00,
        TerrainCellAdminSetBotValues = 0xEC000D00,
        TerrainCellAdminEnableMaterialVeins = 0xEC000E00,
        TerrainCellAdminResetMaterialVeins = 0xEC000F00,
        TerrainCellDetectVeins      = 0xEC001000,
        TerrainCellEndDetectVeins   = 0xEC001100,
        PropBase                    = 0xED00,
        ProjectileBase              = 0xEE00,
        PlacedEffectBase            = 0xEF00,
        TemplateBase                = 0xF000,
        ClientFacialAnim            = 0xF100,
        ZoneProximityFacialAnim     = 0xF200,
        ClientFacialAnimSettings    = 0xF300,
        VoxelConstructionBase       = 0xF400,
        PaletteBase                 = 0xF500,
        CharacterSelectSessionRequest = 0xF600,
        CharacterSelectSessionResponse = 0xF700,
        MailBase                    = 0xF800,
        ClaimsBase                  = 0xF900,
        ClaimVendorBase             = 0x11200,
        BusinessEnvironments        = 0xFA00,
        LootPinataBase              = 0xFB00,
        UpdatePropInteractionState  = 0xFD00,
        ChestBase                   = 0xFF00,
        DebugDrawBase               = 0x10000,
        FogOfWarZoneDataRequest     = 0x10200,
        FogOfWarZoneDataReply       = 0x10300,
        SocialShareBase             = 0x10400,
        HeroicMovement              = 0x10500,
        UpdatePlayerRedeemedPromoBundle = 0x10600,
        MarketplaceImageBase        = 0x10700,
        DelayWrappedPacket          = 0x3800,
        ZoneToWorldToClientWrappedForCharacterNameLookups = 0x10800,
        PreferencePacket            = 0x300,
        ProceduralDataMonitorUpdateRequest = 0x10900,
        ProceduralDataMonitorUpdateResponse = 0x10A00,
        ItemReviewBase              = 0x10B00,
        WorldEditingBase            = 0x10C00,
        ReviveRequest               = 0x200,
        InstancedGameBase           = 0x10D00,
        CombatStateManager          = 0x10E00,
        RequestAllInstancedGames    = 0x10F00,
        RequestZoneData             = 0x11000,
        GameScriptingBase           = 0x11100,
        Notifications               = 0x100,
        WarpNetworkBase             = 0x11300,
        CorrectNpcHeights           = 0x11400,
        KnownPropBase               = 0x11500,
        ArtDataBase                 = 0x11600,
        ToolBase                    = 0x11700,
        ContentBase                 = 0x11800,
        DebugAnimation              = 0x11900,
        DesignBasedStateMachineBase = 0x11A00,
        InteractionBase             = 0x11B00,
        CharacterCustomizationBase  = 0x11C00
    }
}
