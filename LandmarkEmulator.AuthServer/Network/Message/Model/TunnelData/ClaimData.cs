using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData
{
    [TunnelData(TunnelDataType.ClaimData)]
    public class ClaimData : ITunnelData
    {
        public class Claim : ITunnelData
        {
            public ulong Unknown0 { get; set; } // Probably ServerId
            public ulong Unknown1 { get; set; } // Probably ClaimId
            public ulong Unknown2 { get; set; } // Probably Paid Time Remaining?
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }
            public uint Unknown5 { get; set; }
            public bool Unknown6 { get; set; }
            public string Unknown7 { get; set; }
            public double[] Unknown8 { get; set; } = new double[3]; // Probably Position
            public double[] Unknown9 { get; set; } = new double[3]; // Probably Rotation?
            public uint Unknown10 { get; set; }
            public uint Unknown11 { get; set; }

            public uint GetSize()
            {
                return 
                    (uint)
                    (8 +
                    8 +
                    8 +
                    4 +
                    4 +
                    4 +
                    1 +
                    4 + Unknown7.Length +
                    (3 * 8) +
                    (3 * 8) +
                    4 +
                    4);
            }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadULong();
                Unknown2 = reader.ReadULong();
                Unknown3 = reader.ReadUInt();
                Unknown4 = reader.ReadUInt();
                Unknown5 = reader.ReadUInt();
                Unknown6 = reader.ReadBool();
                Unknown7 = reader.ReadString();
                for (int i = 0; i < Unknown8.Length; i++)
                    Unknown8[i] = reader.ReadDouble();
                for (int i = 0; i < Unknown9.Length; i++)
                    Unknown9[i] = reader.ReadDouble();
                Unknown10 = reader.ReadUInt();
                Unknown11 = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                writer.Write(Unknown7);
                for (int i = 0; i < Unknown8.Length; i++)
                    writer.Write(Unknown8[i]);
                for (int i = 0; i < Unknown9.Length; i++)
                    writer.Write(Unknown9[i]);
                writer.Write(Unknown10);
                writer.Write(Unknown11);
            }
        }

        public List<Claim> Claims { get; set; } = new();

        public uint GetSize()
        {
            uint totalSizeOfArray = 0;
            Claims.ForEach(i => totalSizeOfArray += i.GetSize());
            return 4u + totalSizeOfArray;
        }

        public void Read(GamePacketReader reader)
        {
            uint count = reader.ReadUInt();
            for (int i = 0; i < count; i++)
            {
                Claims.Add(new Claim());
            }
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Claims.Count);
            Claims.ForEach(i => i.Write(writer));
        }
    }
}
