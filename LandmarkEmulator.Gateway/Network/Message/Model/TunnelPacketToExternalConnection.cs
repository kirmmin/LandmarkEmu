using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.Gateway.Network.Message.Model
{
    [GatewayMessage(GatewayMessageOpcode.TunnelPacketToExternalConnection, ProtocolVersion.ExternalGatewayApi_3)]
    public class TunnelPacketToExternalConnection : IWritable
    {
        public byte Channel { get; set; }
        public byte[] Data { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteBytes(Data);
        }
    }
}
