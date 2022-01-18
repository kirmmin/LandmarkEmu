using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.Gateway.Network.Message.Model
{
    [GatewayMessage(GatewayMessageOpcode.LoginRequest, ProtocolVersion.ExternalGatewayApi_3)]
    public class LoginRequest : IReadable
    {
        public ulong CharacterId { get; set; }
        public string ServerTicket { get; set; }
        public string ClientProtocol { get; set; }
        public string ClientBuild { get; set; }

        public void Read(GamePacketReader reader)
        {
            CharacterId    = reader.ReadULong();
            ServerTicket   = reader.ReadString();
            ClientProtocol = reader.ReadString();
            ClientBuild    = reader.ReadString();
        }
    }
}
