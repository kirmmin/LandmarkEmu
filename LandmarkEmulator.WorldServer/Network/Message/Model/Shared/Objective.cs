using LandmarkEmulator.Shared.Game;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class ObjectiveEntry : IReadable, IWritable
    {
        public uint Index { get; private set; }
        public Objective Objective { get; private set; } = new();

        public void Read(GamePacketReader reader)
        {
            Index = reader.ReadUInt();
            Objective.Read(reader);
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Index);
            Objective.Write(writer);
        }
    }

    public class Objective : IReadable, IWritable
    {
        // sub_142C22E30
        public class UnknownStruct142C22E30 : IReadable, IWritable
        {
            public uint Unknown0 { get; private set; }
            public uint Unknown1 { get; private set; }
            public uint Unknown2 { get; private set; }
            public uint Unknown3 { get; private set; }
            
            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadUInt();
                Unknown1 = reader.ReadUInt();
                Unknown2 = reader.ReadUInt();
                Unknown3 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
            }
        }

        public uint ObjectiveId { get; private set; }
        public LandmarkText NameId { get; private set; } = new();
        public LandmarkText DescriptionId { get; private set; } = new();
        public RewardBundle ObjectiveRewardBundle { get; private set; } = new();
        public uint Unknown5 { get; private set; }
        public uint Unknown6 { get; private set; }
        public uint Unknown7 { get; private set; }
        public uint Unknown8 { get; private set; }
        public uint Unknown9 { get; private set; }
        public uint Unknown10 { get; private set; }
        public UnknownStruct142C22E30 Unknown11 { get; private set; } = new();
        public byte Unknown12 { get; private set; }
        public uint Unknown13 { get; private set; }

        public void Read(GamePacketReader reader)
        {
            ObjectiveId = reader.ReadUInt();
            NameId = new LandmarkText(reader.ReadUInt());
            DescriptionId = new LandmarkText(reader.ReadUInt());
            
            ObjectiveRewardBundle.Read(reader);

            Unknown5 = reader.ReadUInt();
            Unknown6 = reader.ReadUInt();
            Unknown7 = reader.ReadUInt();
            Unknown8 = reader.ReadUInt();
            Unknown9 = reader.ReadUInt();
            Unknown10 = reader.ReadUInt();

            Unknown11.Read(reader);

            Unknown12 = reader.ReadByte();
            Unknown13 = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ObjectiveId);
            NameId.Write(writer);
            DescriptionId.Write(writer);

            ObjectiveRewardBundle.Write(writer);

            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(Unknown10);

            Unknown11.Write(writer);

            writer.Write(Unknown12);
            writer.Write(Unknown13);
        }
    }
}
