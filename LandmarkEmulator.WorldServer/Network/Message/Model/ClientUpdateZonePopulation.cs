using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientUpdateZonePopulation)]
    public class ClientUpdateZonePopulation : IReadable, IWritable
    {
        public List<byte> Populations { get; set; } = new();
        public uint Unknown0 { get; set; }
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }

        public void Read(GamePacketReader reader)
        {
            ulong populationCount = reader.ReadUInt();

            for (ulong i = 0; i < populationCount; i++)
                Populations.Add(reader.ReadByte());

            Unknown0 = reader.ReadUInt();
            Unknown1 = reader.ReadUInt();
            Unknown2 = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Populations.Count);

            foreach (byte i in Populations)
                writer.Write(i);

            writer.Write(Unknown0);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
        }
    }
}
