using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.InitializationParameters)]
    public class InitializationParameters : IReadable, IWritable
    {
        public string Environment { get; set; } = "";

        public void Read(GamePacketReader reader)
        {
            Environment = reader.ReadString();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Environment);
        }
    }
}
