using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientUpdateZonePopulation)]
    public class ClientUpdateZonePopulation : IReadable, IWritable
    {
        public List<byte> Populations { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            ulong populationCount = reader.ReadULong();

            for (ulong i = 0; i < populationCount; i++)
                Populations.Add(reader.ReadByte());
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((ulong)Populations.Count);

            foreach (byte i in Populations)
                writer.Write(i);
        }
    }
}
