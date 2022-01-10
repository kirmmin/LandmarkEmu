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
            ServerId = reader.ReadULongLE();
            Unknown0 = reader.ReadUIntLE();
            Unknown1 = reader.ReadByte();
            BodyType = reader.ReadUIntLE();
            Gender   = reader.ReadUIntLE();
            Name     = reader.ReadStringLE();
            Unknown3 = reader.ReadUIntLE();
            Unknown4 = reader.ReadUIntLE();
            Unknown5 = reader.ReadULongLE();
            Unknown6 = reader.ReadUIntLE();
            Head     = reader.ReadULongLE();
            Unknown9 = reader.ReadUIntLE();
            Hair     = reader.ReadULongLE();
            Unknown10 = reader.ReadUIntLE();
            FacialHair = reader.ReadULongLE();
            Unknown11 = reader.ReadUIntLE();
            Unknown12 = reader.ReadULongLE();
            Unknown13 = reader.ReadULongLE();
            Unknown14 = reader.ReadByte();
            Unknown15 = reader.ReadUIntLE();
            StartingOutfit = reader.ReadUIntLE();
        }
    }

    [AuthMessage(AuthMessageOpcode.CharacterCreateRequest, ProtocolVersion.LoginUdp_9)]
    public class CharacterCreateRequest9 : IReadable
    {
        public ulong ServerId { get; set; }
        public uint Unknown0 { get; set; }
        public byte Unknown1 { get; set; }
        public uint BodyType { get; set; }
        public uint Gender { get; set; }
        public string Name { get; set; }
        public Dictionary<uint, uint> CustomisationOptions { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            ServerId = reader.ReadULongLE();
            Unknown0 = reader.ReadUIntLE();
            Unknown1 = reader.ReadByte();
            BodyType = reader.ReadUIntLE();
            Gender   = reader.ReadUIntLE();
            Name     = reader.ReadStringLE();

            uint customisationCount = reader.ReadUIntLE();
            for (uint i = 0; i < customisationCount; i++)
            {
                var index = reader.ReadUIntLE();
                var slotId = reader.ReadUIntLE();
                var slotValue = reader.ReadUIntLE();

                CustomisationOptions.TryAdd(slotId, slotValue);
            }
        }
    }
}
