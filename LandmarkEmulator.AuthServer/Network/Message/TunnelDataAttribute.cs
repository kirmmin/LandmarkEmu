using LandmarkEmulator.AuthServer.Network.Message.Static;
using System;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TunnelDataAttribute : Attribute
    {
        public TunnelDataType Type { get; }

        public TunnelDataAttribute(TunnelDataType type)
        {
            Type = type;
        }
    }
}
