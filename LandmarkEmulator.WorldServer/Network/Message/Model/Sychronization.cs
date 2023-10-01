using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.Synchronization)]
    public class Synchronization : IReadable, IWritable
    {
        public double Time1 { get; set; }
        public double Time2 { get; set; }
        public double ClientTime { get; set; }
        public double ServerTime { get; set; }
        public double ServerTime2 { get; set; }
        public double Time3 { get; set; }
        public bool Unknown0 { get; set; }

        public void Read(GamePacketReader reader)
        {
            Time1 = reader.ReadDouble();
            Time2 = reader.ReadDouble();
            ClientTime = reader.ReadDouble();
            ServerTime = reader.ReadDouble();
            ServerTime2 = reader.ReadDouble();
            Time3 = reader.ReadDouble();
            Unknown0 = reader.ReadBool();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Time1);
            writer.Write(Time2);
            writer.Write(ClientTime);
            writer.Write(ServerTime);
            writer.Write(ServerTime2);
            writer.Write(Time3);
            writer.Write(Unknown0);
        }
    }
}
