using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientGameSettings)]
    public class ClientGameSettings : IReadable, IWritable
    {
        public uint Unknown0 { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public uint Unknown4 { get; set; }
        public bool Unknown5 { get; set; }
        public float Unknown6 { get; set; }
        public float Unknown7 { get; set; }
        public float Unknown8 { get; set; }
        public float Unknown9 { get; set; }

        public void Read(GamePacketReader reader)
        {
            Unknown0 = reader.ReadUInt();
            Unknown1 = reader.ReadUInt();
            Unknown2 = reader.ReadUInt();
            Unknown3 = reader.ReadUInt();
            Unknown4 = reader.ReadUInt();
            Unknown5 = reader.ReadBool();
            Unknown6 = reader.ReadSingle();
            Unknown7 = reader.ReadSingle();
            Unknown8 = reader.ReadSingle();
            Unknown9 = reader.ReadSingle();
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
            writer.Write(Unknown8);
            writer.Write(Unknown9);
        }
    }
}
