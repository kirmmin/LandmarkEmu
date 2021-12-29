using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GameMessageHandlerAttribute : Attribute
    {
        public GameMessageOpcode Opcode { get; }

        public GameMessageHandlerAttribute(GameMessageOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
