using LandmarkEmulator.Shared.Network.Message;
using System;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthMessageAttribute : Attribute
    {
        public AuthMessageOpcode Opcode { get; }
        public MessageDirection Direction { get; }

        public AuthMessageAttribute(AuthMessageOpcode opcode, MessageDirection direction)
        {
            Opcode = opcode;
            Direction = direction;
        }
    }
}
