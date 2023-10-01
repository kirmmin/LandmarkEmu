using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message.Model.Shared;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.SendZoneDetails)]
    public class SendZoneDetails : IReadable, IWritable
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
        public Sky SkyData { get; set; } = new();

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

            SkyData.Read(reader);
        }

        public void Write(GamePacketWriter writer)
        {

        }
    }
}
