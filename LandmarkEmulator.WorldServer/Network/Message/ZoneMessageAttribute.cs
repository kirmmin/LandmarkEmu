using LandmarkEmulator.Gateway.Network.Message;
using System;

namespace LandmarkEmulator.WorldServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ZoneMessageAttribute : Attribute
    {
        public ZoneMessageOpcode Opcode { get; }
        public ClientProtocol Version { get; }
        public bool PrependSize { get; }

        public ZoneMessageAttribute(ZoneMessageOpcode opcode, ClientProtocol version = ClientProtocol.ClientProtocol_ALL, bool prependSize = false)
        {
            Opcode = opcode;
            Version = version;
            PrependSize = prependSize;
        }
    }
}
