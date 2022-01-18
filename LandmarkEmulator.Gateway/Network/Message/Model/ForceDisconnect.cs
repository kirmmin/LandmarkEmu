using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.Gateway.Network.Message.Model
{
    [GatewayMessage(GatewayMessageOpcode.ForceDisconnect, ProtocolVersion.ExternalGatewayApi_3)]
    public class ForceDisconnect : IWritable
    {
        public uint Reason { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Reason);    
        }
    }
}
