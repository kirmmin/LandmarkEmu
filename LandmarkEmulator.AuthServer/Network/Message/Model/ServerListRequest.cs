using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.ServerListRequest, MessageDirection.Client)]
    public class ServerListRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
