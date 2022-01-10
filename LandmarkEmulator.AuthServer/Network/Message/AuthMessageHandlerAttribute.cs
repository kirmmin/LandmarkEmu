using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthMessageHandlerAttribute : Attribute
    {
        public AuthMessageOpcode Opcode { get; }
        public ProtocolVersion ProtocolVersion { get; }

        public AuthMessageHandlerAttribute(AuthMessageOpcode opcode, ProtocolVersion version = ProtocolVersion.LOGIN_ALL)
        {
            Opcode          = opcode;
            ProtocolVersion = version;
        }
    }
}
