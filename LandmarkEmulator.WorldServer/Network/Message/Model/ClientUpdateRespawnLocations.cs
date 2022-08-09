using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientUpdateRespawnLocations)]
    public class ClientUpdateRespawnLocations : IReadable, IWritable
    {
        public class RespawnLocation : IReadable, IWritable
        {
            public ulong Unknown0 { get; set; }
            public byte Unknown1 { get; set; }
            public ulong Unknown2 { get; set; }
            public ulong Unknown3 { get; set; }
            public ulong Unknown4 { get; set; }
            public ulong Unknown5 { get; set; }
            public ulong Unknown6 { get; set; }
            public ulong Unknown7 { get; set; }
            public ushort Unknown8 { get; set; }

            public void Read(GamePacketReader reader)
            {
                Unknown0 = reader.ReadULong();
                Unknown1 = reader.ReadByte();
                Unknown2 = reader.ReadULong();
                Unknown3 = reader.ReadULong();
                Unknown4 = reader.ReadULong();
                Unknown5 = reader.ReadULong();
                Unknown6 = reader.ReadULong();
                Unknown7 = reader.ReadULong();
                Unknown8 = reader.ReadUShort();
            }

            public void Write(GamePacketWriter writer)
            {

            }
        }

        public List<RespawnLocation> RespawnLocations { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            uint respawnCount = reader.ReadUInt();

            for (ulong i = 0; i < respawnCount; i++)
            {
                var location = new RespawnLocation();
                location.Read(reader);
                RespawnLocations.Add(location);
            }
        }

        public void Write(GamePacketWriter writer)
        {
            
        }
    }
}
