using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.TunnelPacketClientToServer, ProtocolVersion.LOGIN_ALL)]
    public class TunnelPacketClientToServer : TunnelPacket
    {
    }
}
