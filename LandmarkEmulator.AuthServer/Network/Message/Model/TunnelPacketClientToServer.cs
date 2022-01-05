using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.TunnelPacketClientToServer, MessageDirection.Client)]
    public class TunnelPacketClientToServer : TunnelPacket
    {
    }
}
