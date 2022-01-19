using LandmarkEmulator.Gateway.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ZoneMessageAttribute : Attribute
    {
        public ZoneMessageOpcode Opcode { get; }
        public ClientProtocol Version { get; }

        public ZoneMessageAttribute(ZoneMessageOpcode opcode, ClientProtocol version = ClientProtocol.ClientProtocol_ALL)
        {
            Opcode = opcode;
            Version = version;
        }
    }
}
