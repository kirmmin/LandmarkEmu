using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoRequest, MessageDirection.Client)]
    public class CharacterSelectInfoRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
