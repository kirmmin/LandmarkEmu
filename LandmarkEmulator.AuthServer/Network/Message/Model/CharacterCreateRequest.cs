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
        public byte Unknown1 { get; set; }
        public uint BodyType { get; set; }
        public uint Gender { get; set; }
        public string Name { get; set; }
        public uint Unknown3 { get; set; }
        public uint Unknown4 { get; set; }
        public ulong Unknown5 { get; set; }
        public uint Unknown6 { get; set; }
        public ulong Head { get; set; }
        public uint Unknown9 { get; set; }
        public ulong Hair { get; set; }
        public uint Unknown10 { get; set; }
        public ulong FacialHair { get; set; }
        public uint Unknown11 { get; set; }
        public ulong Unknown12 { get; set; }
        public ulong Unknown13 { get; set; }
        public byte Unknown14 { get; set; }
        public uint Unknown15 { get; set; }
        public uint StartingOutfit { get; set; }

        public void Read(GamePacketReader reader)
        {
            ServerId = reader.ReadULong();
            Unknown0 = reader.ReadUInt();
            Unknown1 = reader.ReadByte();
            BodyType = reader.ReadUInt();
            Gender   = reader.ReadUInt();
            Name     = reader.ReadString();
            Unknown3 = reader.ReadUInt();
            Unknown4 = reader.ReadUInt();
            Unknown5 = reader.ReadULong();
            Unknown6 = reader.ReadUInt();
            Head     = reader.ReadULong();
            Unknown9 = reader.ReadUInt();
            Hair     = reader.ReadULong();
            Unknown10 = reader.ReadUInt();
            FacialHair = reader.ReadULong();
            Unknown11 = reader.ReadUInt();
            Unknown12 = reader.ReadULong();
            Unknown13 = reader.ReadULong();
            Unknown14 = reader.ReadByte();
            Unknown15 = reader.ReadUInt();
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
