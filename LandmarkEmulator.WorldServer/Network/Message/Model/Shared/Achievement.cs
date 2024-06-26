﻿using LandmarkEmulator.Shared.Game;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LandmarkEmulator.WorldServer.Network.Message.Model.SendSelfToClient;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class Achievement : IReadable, IWritable
    {
        public uint AchievementId { get; private set; }
        public byte Unknown1 { get; private set; }
        public LandmarkText NameId { get; private set; }
        public LandmarkText DescriptionId { get; private set; }
        public ulong TimeStarted { get; private set; }
        public ulong TimeFinished { get; private set; }
        public float Progress { get; private set; }

        public RewardBundle AchievementRewardBundle { get; private set; } = new();

        public uint ObjectiveCount { get; private set; }
        public List<ObjectiveEntry> AchievementObjectives { get; private set; } = new();

        public uint Unknown9 { get; private set; }
        public uint Unknown10 { get; private set; }
        public uint Unknown11 { get; private set; }
        public uint Unknown12 { get; private set; }
        public uint Unknown13 { get; private set; }
        public byte Unknown14 { get; private set; }
        public uint Unknown15 { get; private set; }
        public bool Unknown16 { get; private set; }

        public uint Unknown17 { get; private set; }
        public uint Unknown18 { get; private set; }
        public uint Unknown19 { get; private set; }
        public uint Unknown20 { get; private set; }
        public uint Unknown21 { get; private set; }
        public uint Unknown22 { get; private set; }
        public uint Unknown23 { get; private set; }
        public uint Unknown24 { get; private set; }
        public uint Unknown25 { get; private set; }
        public uint Unknown26 { get; private set; }

        public bool Unknown27 { get; private set; }
        public bool Unknown28 { get; private set; }
        public bool Unknown29 { get; private set; }

        public UnknownStruct142C34B60 Unknown30 { get; private set; } = new();
        public UnknownStruct142C363D0 Unknown31 { get; private set; } = new();
        public UnknownStruct142C34F90 Unknown32 { get; private set; } = new();

        public uint Unknown33 { get; private set; }

        public void Read(GamePacketReader reader)
        {
            AchievementId = reader.ReadUInt();
            Unknown1 = reader.ReadByte();
            NameId = new LandmarkText(reader.ReadUInt());
            DescriptionId = new LandmarkText(reader.ReadUInt());
            TimeStarted = reader.ReadULong();
            TimeFinished = reader.ReadULong();
            Progress = reader.ReadSingle();

            AchievementRewardBundle.Read(reader);

            ObjectiveCount = reader.ReadUInt();
            for (int i = 0; i < ObjectiveCount; i++)
            {
                ObjectiveEntry objective = new();
                objective.Read(reader);
                AchievementObjectives.Add(objective);
            }

            Unknown9 = reader.ReadUInt();
            Unknown10 = reader.ReadUInt();
            Unknown11 = reader.ReadUInt();
            Unknown12 = reader.ReadUInt();
            Unknown13 = reader.ReadUInt();
            Unknown14 = reader.ReadByte();
            Unknown15 = reader.ReadUInt();
            Unknown16 = reader.ReadBool();

            Unknown17 = reader.ReadUInt();
            Unknown18 = reader.ReadUInt();
            Unknown19 = reader.ReadUInt();
            Unknown20 = reader.ReadUInt();
            Unknown21 = reader.ReadUInt();
            Unknown22 = reader.ReadUInt();
            Unknown23 = reader.ReadUInt();
            Unknown24 = reader.ReadUInt();
            Unknown25 = reader.ReadUInt();
            Unknown26 = reader.ReadUInt();

            Unknown27 = reader.ReadBool();
            Unknown28 = reader.ReadBool();
            Unknown29 = reader.ReadBool();

            Unknown30.Read(reader);
            Unknown31.Read(reader);
            Unknown32.Read(reader);

            Unknown33 = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AchievementId);
            writer.Write(Unknown1);
            NameId.Write(writer);
            DescriptionId.Write(writer);
            writer.Write(TimeStarted);
            writer.Write(TimeFinished);
            writer.Write(Progress);

            AchievementRewardBundle.Write(writer);

            writer.Write((uint)AchievementObjectives.Count);
            foreach (ObjectiveEntry objective in AchievementObjectives)
                objective.Write(writer);

            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);
            writer.Write(Unknown12);
            writer.Write(Unknown13);
            writer.Write(Unknown14);
            writer.Write(Unknown15);
            writer.Write(Unknown16);

            writer.Write(Unknown17);
            writer.Write(Unknown18);
            writer.Write(Unknown19);
            writer.Write(Unknown20);
            writer.Write(Unknown21);
            writer.Write(Unknown22);
            writer.Write(Unknown23);
            writer.Write(Unknown24);
            writer.Write(Unknown25);
            writer.Write(Unknown26);

            writer.Write(Unknown27);
            writer.Write(Unknown28);
            writer.Write(Unknown29);

            Unknown30.Write(writer);
            Unknown31.Write(writer);
            Unknown32.Write(writer);

            writer.Write(Unknown33);
        }
    }
}
