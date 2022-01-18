using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.Gateway.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GatewayMessageAttribute : Attribute
    {
        public GatewayMessageOpcode Opcode { get; }
        public ProtocolVersion Version { get; }

        public GatewayMessageAttribute(GatewayMessageOpcode opcode, ProtocolVersion version)
        {
            Opcode = opcode;
            Version = version;
        }
    }
}
