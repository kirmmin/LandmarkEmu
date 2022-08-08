using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientIsReady)]
    public class ClientIsReady : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
