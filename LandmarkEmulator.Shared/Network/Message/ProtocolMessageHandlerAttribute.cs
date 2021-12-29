using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ProtocolMessageHandlerAttribute : Attribute
    {
        public ProtocolMessageOpcode Opcode { get; }

        public ProtocolMessageHandlerAttribute(ProtocolMessageOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
