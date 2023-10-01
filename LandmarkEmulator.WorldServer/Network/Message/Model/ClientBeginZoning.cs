using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message.Model.Shared;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientBeginZoning)]
    public class ClientBeginZoning : IReadable, IWritable
    {
        public string ZoneName { get; set; }
        public uint ZoneType { get; set; }
        public float[] Position { get; set; } = new float[4];
        public float[] Rotation { get; set; } = new float[4] { 0f, 0f, 0f, 1f };
        public string SkyXml { get; set; }
        public byte Unknown3 { get; set; }
        public uint WorldId { get; set; }
        public uint WorldNameId { get; set; }
        public uint ZoneId { get; set; }
        public uint ZoneNameId { get; set; }
        public uint Unknown8 { get; set; }
        public uint Unknown9 { get; set; }
        public bool Unknown10 { get; set; }
        public bool Unknown11 { get; set; }

        public Sky SkyData { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            ZoneName = reader.ReadString();
            ZoneType = reader.ReadUInt();
            for (int i = 0; i < 4; i++)
                Position[i] = reader.ReadSingle();
            for (int i = 0; i < 4; i++)
                Rotation[i] = reader.ReadSingle();
            SkyXml = reader.ReadString();
            Unknown3 = reader.ReadByte();
            WorldId = reader.ReadUInt();
            WorldNameId = reader.ReadUInt();
            ZoneId = reader.ReadUInt();
            ZoneNameId = reader.ReadUInt();
            Unknown8 = reader.ReadUInt();
            Unknown9 = reader.ReadUInt();
            Unknown10 = reader.ReadBool();
            Unknown11 = reader.ReadBool();

            SkyData.Read(reader);
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(ZoneName);
            writer.Write(ZoneType);
            for (int i = 0; i < 4; i++)
                writer.Write(Position[i]);
            for (int i = 0; i < 4; i++)
                writer.Write(Rotation[i]);
            writer.Write(SkyXml);
            writer.Write(Unknown3);
            writer.Write(WorldId);
            writer.Write(WorldNameId);
            writer.Write(ZoneId);
            writer.Write(ZoneNameId);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
            writer.Write(Unknown10);
            writer.Write(Unknown11);

            SkyData.Write(writer);
        }
    }
}
