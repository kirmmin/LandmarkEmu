using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.Gateway.Network.Message
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GatewayMessageHandlerAttribute : Attribute
    {
        public GatewayMessageOpcode Opcode { get; }
        public ProtocolVersion ProtocolVersion { get; }

        public GatewayMessageHandlerAttribute(GatewayMessageOpcode opcode, ProtocolVersion version)
        {
            Opcode = opcode;
            ProtocolVersion = version;
        }
    }
}
