using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GameMessageAttribute : Attribute
    {
        public GameMessageOpcode Opcode { get; }
        public MessageDirection Direction { get; }

        public GameMessageAttribute(GameMessageOpcode opcode, MessageDirection direction)
        {
            Opcode = opcode;
            Direction = direction;
        }
    }
}
