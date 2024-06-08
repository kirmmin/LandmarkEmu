using LandmarkEmulator.Shared.Game;

namespace LandmarkEmulator.Shared.GameTable.Model
{
    public class ProfileDefinitionEntry
    {
        // Human: 46^571666^0^0^3^0^0^0^0^2514^0^0^0^0^0^0^0^1^1^0^0^0^0^0^1^^390^392^0^0^0^2.8^2.75^0.4^1^11294^2512^1.4^15^0^0^0^0^0^0^0^0^3^3^3^0^
        public uint Id { get; set; } // 46
        public uint NameId { get; set; } // 571666
        public uint DescriptionId { get; set; } // ^0
        public uint RequirementId { get; set; } // ^0
        public uint ProfileType { get; set; } // ^3
        public uint StartingEquipmentSetId { get; set; } // ^0
        public uint IconId { get; set; } // ^0
        public uint Category { get; set; } // ^0
        public uint AbilityBgImageSetId { get; set; } // ^0
        public uint SwitchProfileEffectId { get; set; } // ^2514
        public uint SwitchProfileAnimation { get; set; } // ^0
        public bool MembersOnly { get; set; } // ^0
        public uint DisplayedStatSetId { get; set; } // ^0 
        public uint PreviewHintStringId { get; set; } // ^0
        public bool ShowPreview { get; set; } // ^0
        public bool TrialEnabled { get; set; } // ^0
        public uint TrainerLocatorQuestId { get; set; } // ^0
        public float ExperienceModifier { get; set; } // ^1
        public float CoinModifier { get; set; } // ^1
        public uint TrialLevelCap { get; set; } // ^0
        public bool GrandfatheredFree { get; set; } // ^0
        public bool AutoGrant { get; set; } // ^0
        public uint WieldTypeOverride { get; set; } // ^0
        public uint ContentId { get; set; } // ^0
        public uint FactionId { get; set; } // ^1
        public string Identifier { get; set; } // ^
        public uint MaleModelId { get; set; } // ^390
        public uint FemaleModelId { get; set; } // ^392
        public bool NoCrouch { get; set; } // ^0
        public bool NoJump { get; set; } // ^0
        public bool NoStrafe { get; set; } // ^0
        public float StandCameraHeight { get; set; } // ^2.8
        public float LineOfSightHeight { get; set; } // ^2.75
        public float CapsuleRadius { get; set; } // ^0.4
        public uint UiModelCameraId { get; set; } // ^1
        public uint SpawnEffectId { get; set; } // ^11294
        public uint DespawnEffectId { get; set; } // ^2512
        public float CrouchCameraHeight { get; set; } // ^1.4
        public float Mass { get; set; } // ^15
        public uint AcquireSec { get; set; } // ^0
        public uint CurrencyType { get; set; } // ^0
        public uint Cost { get; set; } // ^0
        public uint WeaponSetId01 { get; set; } // ^0
        public uint WeaponSetId02 { get; set; } // ^0
        public uint WeaponSetId03 { get; set; } // ^0
        public uint WeaponSetId04 { get; set; } // ^0
        public uint WeaponSetId05 { get; set; } // ^0
        public uint CharSize { get; set; } // ^3
        public uint Armor { get; set; } // ^3
        public uint Feet { get; set; } // ^3
        public float RepairableDamageAngle { get; set; } // ^0
    }
}
