using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [Flags]
    public enum MessageDirection
    {
        Client = 0x01,
        Server = 0x02
    }
}
