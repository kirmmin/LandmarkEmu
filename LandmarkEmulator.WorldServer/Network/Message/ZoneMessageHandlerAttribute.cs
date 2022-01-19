using LandmarkEmulator.Gateway.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ZoneMessageHandlerAttribute : Attribute
    {
        public ZoneMessageOpcode Opcode { get; }
        public ClientProtocol ProtocolVersion { get; }

        public ZoneMessageHandlerAttribute(ZoneMessageOpcode opcode, ClientProtocol version = ClientProtocol.ClientProtocol_ALL)
        {
            Opcode = opcode;
            ProtocolVersion = version;
        }
    }
}
