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
                Unknown0 = reader.ReadULongLE();
                Unknown1 = reader.ReadULongLE();
                Unknown2 = reader.ReadULongLE();
                Unknown3 = reader.ReadUIntLE();
                Unknown4 = reader.ReadUIntLE();
                Unknown5 = reader.ReadUIntLE();
                Unknown6 = reader.ReadBool();
                Unknown7 = reader.ReadStringLE();
                for (int i = 0; i < Unknown8.Length; i++)
                    Unknown8[i] = reader.ReadDoubleLE();
                for (int i = 0; i < Unknown9.Length; i++)
                    Unknown9[i] = reader.ReadDoubleLE();
                Unknown10 = reader.ReadUIntLE();
                Unknown11 = reader.ReadUIntLE();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(Unknown0);
                writer.WriteLE(Unknown1);
                writer.WriteLE(Unknown2);
                writer.WriteLE(Unknown3);
                writer.WriteLE(Unknown4);
                writer.WriteLE(Unknown5);
                writer.Write(Unknown6);
                writer.WriteLE(Unknown7);
                for (int i = 0; i < Unknown8.Length; i++)
                    writer.WriteLE(Unknown8[i]);
                for (int i = 0; i < Unknown9.Length; i++)
                    writer.WriteLE(Unknown9[i]);
                writer.WriteLE(Unknown10);
                writer.WriteLE(Unknown11);
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
            uint count = reader.ReadUIntLE();
            for (int i = 0; i < count; i++)
            {
                Claims.Add(new Claim());
            }
        }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE((uint)Claims.Count);
            Claims.ForEach(i => i.Write(writer));
        }
    }
}
