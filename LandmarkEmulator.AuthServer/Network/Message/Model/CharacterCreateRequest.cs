using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterCreateRequest, ProtocolVersion.LoginUdp_10)]
    public class CharacterCreateRequest : IReadable
    {
        public ulong ServerId { get; set; }
        public uint Unknown0 { get; set; }
        public byte EmpireId { get; set; }
        public uint ProfileTypeId { get; set; }
        public uint Gender { get; set; }
        public string Name { get; set; }
        public List<(uint, uint, uint)> CustomisationOptions { get; set; } = new();
        public uint SkinTint { get; set; }
        public uint StartingOutfit { get; set; }

        public void Read(GamePacketReader reader)
        {
            ServerId = reader.ReadULong();
            Unknown0 = reader.ReadUInt();
            EmpireId = reader.ReadByte();
            ProfileTypeId = reader.ReadUInt();
            Gender   = reader.ReadUInt();
            Name     = reader.ReadString();
            uint customisationCount = reader.ReadUInt();
            for (uint i = 0; i < customisationCount; i++)
            {
                var slotId = reader.ReadUInt();
                var optionId = reader.ReadUInt();
                var tintId = reader.ReadUInt();

                CustomisationOptions.Add(new(slotId, optionId, tintId));
            }

            SkinTint = reader.ReadUInt();
            StartingOutfit = reader.ReadUInt();
        }
    }

    [AuthMessage(AuthMessageOpcode.CharacterCreateRequest, ProtocolVersion.LoginUdp_9)]
    public class CharacterCreateRequest9 : IReadable
    {
        public ulong ServerId { get; set; }
        public uint Unknown0 { get; set; }
        public byte EmpireId { get; set; }
        public uint ProfileTypeId { get; set; }
        public uint Gender { get; set; }
        public string Name { get; set; }
        public List<(uint, uint, uint)> CustomisationOptions { get; set; } = new();
        public uint SkinTint { get; set; }

        public void Read(GamePacketReader reader)
        {
            ServerId = reader.ReadULong();
            Unknown0 = reader.ReadUInt();
            EmpireId = reader.ReadByte();
            ProfileTypeId = reader.ReadUInt();
            Gender   = reader.ReadUInt();
            Name     = reader.ReadString();

            uint customisationCount = reader.ReadUInt();
            for (uint i = 0; i < customisationCount; i++)
            {
                var slotId = reader.ReadUInt();
                var optionId = reader.ReadUInt();
                var tintId = reader.ReadUInt();

                CustomisationOptions.Add(new (slotId, optionId, tintId));
            }
            
            SkinTint = reader.ReadUInt();
        }
    }
}
