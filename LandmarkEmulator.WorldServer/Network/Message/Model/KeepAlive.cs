using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.KeepAlive)]
    public class KeepAlive: IReadable, IWritable
    {
        public uint GameTime { get; set; }

        public void Read(GamePacketReader reader)
        {
            GameTime = reader.ReadUInt();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(GameTime);
        }
    }
}
