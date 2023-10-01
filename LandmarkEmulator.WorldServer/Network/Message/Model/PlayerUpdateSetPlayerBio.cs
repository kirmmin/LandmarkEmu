using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.PlayerUpdateSetPlayerBio)]
    public class PlayerUpdateSetPlayerBio : IReadable, IWritable
    {
        public void Read(GamePacketReader reader)
        {
        }

        public void Write(GamePacketWriter writer)
        {

        }
    }
}
