using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientFinishedLoading)]
    public class ClientFinishedLoading : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
