using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtocolMessageAttribute : Attribute
    {
        public ProtocolMessageOpcode Opcode { get; }
        public bool UseEncryption { get; }

        public ProtocolMessageAttribute(ProtocolMessageOpcode opcode, bool useEncryption)
        {
            Opcode = opcode;
            UseEncryption = useEncryption;
        }
    }
}
