using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterSelectInfoRequest, ProtocolVersion.LOGIN_ALL)]
    public class CharacterSelectInfoRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
