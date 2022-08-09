using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ReferenceDataItemClassDefinitions)]
    public class ReferenceDataItemClassDefinitions : IReadable, IWritable
    {
        public List<uint> Classes { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            uint classCount = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Classes.Count);
        }
    }
}
