using System;
using System.Collections.Generic;

namespace LandmarkEmulator.Database.Character.Model
{
    public class CharacterModel
    {
        public ulong Id { get; set; }
        public ulong AccountId { get; set; }
        public string Name { get; set; }
        public byte Gender { get; set; }
        public byte Race { get; set; }
        public uint SkinTint { get; set; }
        public uint ProfileTypeId { get; set; }
        public ulong LastServerId { get; set; }
        public DateTime? LastOnline { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string OriginalName { get; set; }

        public ICollection<CharacterCustomisationModel> Customisation { get; set; } = new HashSet<CharacterCustomisationModel>();
    }
}
