using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model.Shared
{
    public class StatData : IReadable, IWritable
    {
        public uint StatId { get; set; }
        public byte Type { get; set; }
        public float Base { get; set; }
        public float Modifier { get; set; }

        public void Read(GamePacketReader reader)
        {
            StatId = reader.ReadUInt();
            Type = reader.ReadByte();

            switch (Type)
            {
                case 0:
                    Base = (float)reader.ReadUInt();
                    Modifier = (float)reader.ReadUInt();
                    break;
                case 1:
                default:
                    Base = reader.ReadSingle();
                    Modifier = reader.ReadSingle();
                    break;
            }
        }

        public void Write(GamePacketWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
