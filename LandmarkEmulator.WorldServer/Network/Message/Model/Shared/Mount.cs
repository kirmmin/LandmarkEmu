using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class Mount : IReadable, IWritable
    {
        public uint Unknown0 { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }
        public ulong Unknown3 { get; set; }
        public bool Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public string Unknown6 { get; set; } = "";

        public void Read(GamePacketReader reader)
        {
            Unknown0 = reader.ReadUInt();
            Unknown1 = reader.ReadUInt();
            Unknown2 = reader.ReadUInt();
            Unknown3 = reader.ReadULong();
            Unknown4 = reader.ReadBool();
            Unknown5 = reader.ReadUInt();
            Unknown6 = reader.ReadString();
        }

        public void Write(GamePacketWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
