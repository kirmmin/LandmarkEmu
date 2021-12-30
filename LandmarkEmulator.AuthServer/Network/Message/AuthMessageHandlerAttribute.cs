using System;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthMessageHandlerAttribute : Attribute
    {
        public AuthMessageOpcode Opcode { get; }

        public AuthMessageHandlerAttribute(AuthMessageOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
