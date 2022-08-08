using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.SendZoneDetails)]
    public class SendZoneDetails : IReadable
    {
        public string Unknown0 { get; set; }
        public uint Unknown1 { get; set; }
        public bool Unknown2 { get; set; }
        public string Unknown3 { get; set; }
        public uint Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public ulong Unknown6 { get; set; }
        public uint Unknown7 { get; set; }
        public uint Unknown8 { get; set; }
        public uint Unknown9 { get; set; }
        public Sky SkyData { get; set; }

        public class Sky : IReadable
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
            public float[] Unknown22 { get; set; } = new float[4];

            public void Read(GamePacketReader reader)
            {

            }
        }

        public void Read(GamePacketReader reader)
        {
            Unknown0 = reader.ReadString();
            Unknown1 = reader.ReadUInt();
            Unknown2 = reader.ReadBool();
            Unknown3 = reader.ReadString();
            Unknown4 = reader.ReadUInt();
            Unknown5 = reader.ReadUInt();
            Unknown6 = reader.ReadULong();
            Unknown7 = reader.ReadUInt();
            Unknown8 = reader.ReadUInt();
            Unknown9 = reader.ReadUInt();
        }
    }
}
