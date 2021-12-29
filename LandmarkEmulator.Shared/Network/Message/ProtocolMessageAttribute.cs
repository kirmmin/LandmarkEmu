using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtocolMessageAttribute : Attribute
    {
        public ProtocolMessageOpcode Opcode { get; }

        public ProtocolMessageAttribute(ProtocolMessageOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
