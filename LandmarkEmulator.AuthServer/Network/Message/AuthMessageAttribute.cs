using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthMessageAttribute : Attribute
    {
        public AuthMessageOpcode Opcode { get; }
        public ProtocolVersion Version{ get; }

        public AuthMessageAttribute(AuthMessageOpcode opcode, ProtocolVersion version)
        {
            Opcode  = opcode;
            Version = version;
        }
    }
}
