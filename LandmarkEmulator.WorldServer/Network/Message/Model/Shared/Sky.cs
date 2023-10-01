using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class Sky : IReadable, IWritable
    {
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public string Unknown4 { get; set; }
        public string Unknown5 { get; set; }
        public ulong Unknown6 { get; set; }
        public double[] Unknown7 { get; set; } = new double[3];
        public double[] Unknown8 { get; set; } = new double[3];
        public bool Unknown9 { get; set; }
        public bool Unknown10 { get; set; }
        public string Unknown11 { get; set; }
        public uint Unknown12 { get; set; }
        public string Unknown13 { get; set; }
        public uint Unknown14 { get; set; }
        public string Unknown15 { get; set; }
        public uint Unknown16 { get; set; }
        public uint Unknown17 { get; set; }
        public uint Unknown18 { get; set; }
        public uint Unknown19 { get; set; }
        public uint Unknown20 { get; set; }
        public uint Unknown21 { get; set; }
        public uint[] Unknown22 { get; set; } = new uint[4];
        public uint Unknown23 { get; set; }
        public uint Unknown24 { get; set; }
        public uint Unknown25 { get; set; }
        public uint Unknown26 { get; set; }
        public ulong Unknown27 { get; set; }
        public uint Unknown28 { get; set; }
        public uint Unknown29 { get; set; }
        public bool Unknown30 { get; set; }
        public uint Unknown31 { get; set; }
        public bool Unknown32 { get; set; }
        public uint Unknown33 { get; set; }
        public uint Unknown34 { get; set; }
        public uint Unknown35 { get; set; }
        public string Unknown36 { get; set; }

        public void Read(GamePacketReader reader)
        {
            Unknown1 = reader.ReadUInt();
            Unknown2 = reader.ReadUInt();
            Unknown3 = reader.ReadUInt();
            Unknown4 = reader.ReadString();
            Unknown5 = reader.ReadString();
            Unknown6 = reader.ReadULong();
            for (int i = 0; i < 3; i++)
                Unknown7[i] = reader.ReadDouble();
            for (int i = 0; i < 3; i++)
                Unknown8[i] = reader.ReadDouble();
            Unknown9 = reader.ReadBool();
            Unknown10 = reader.ReadBool();
            Unknown11 = reader.ReadString();
            Unknown12 = reader.ReadUInt();
            Unknown13 = reader.ReadString();
            Unknown14 = reader.ReadUInt();
            Unknown15 = reader.ReadString();
            Unknown16 = reader.ReadUInt();
            Unknown17 = reader.ReadUInt();
            Unknown18 = reader.ReadUInt();
            Unknown19 = reader.ReadUInt();
            Unknown20 = reader.ReadUInt();
            Unknown21 = reader.ReadUInt();
            for (int i = 0; i < 4; i++)
                Unknown22[i] = reader.ReadUInt();
            Unknown23 = reader.ReadUInt();
            Unknown24 = reader.ReadUInt();
            Unknown25 = reader.ReadUInt();
            Unknown26 = reader.ReadUInt();
            Unknown27 = reader.ReadULong();
            Unknown28 = reader.ReadUInt();
            Unknown29 = reader.ReadUInt();
            Unknown30 = reader.ReadBool();
            Unknown31 = reader.ReadUInt();
            Unknown32 = reader.ReadBool();
            Unknown33 = reader.ReadUInt();
            Unknown34 = reader.ReadUInt();
            Unknown35 = reader.ReadUInt();
            Unknown36 = reader.ReadString();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            for (int i = 0; i < 3; i++)
                writer.Write(Unknown7[i]);
            for (int i = 0; i < 3; i++)
                writer.Write(Unknown8[i]);
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
            for (int i = 0; i < 4; i++)
                writer.Write(Unknown22[i]);
            writer.Write(Unknown23);
            writer.Write(Unknown24);
            writer.Write(Unknown25);
            writer.Write(Unknown26);
            writer.Write(Unknown27);
            writer.Write(Unknown28);
            writer.Write(Unknown29);
            writer.Write(Unknown30);
            writer.Write(Unknown31);
            writer.Write(Unknown32);
            writer.Write(Unknown33);
            writer.Write(Unknown34);
            writer.Write(Unknown35);
            writer.Write(Unknown36);
        }
    }
}
