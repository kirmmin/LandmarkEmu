using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.Gateway.Network.Message.Model
{
    [GatewayMessage(GatewayMessageOpcode.LoginReply, ProtocolVersion.ExternalGatewayApi_3)]
    public class LoginReply : IWritable
    {
        public bool LoggedIn { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(LoggedIn);
        }
    }
}
