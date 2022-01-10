using System;

namespace LandmarkEmulator.Shared.Network.Message
{
    [Flags]
    public enum ProtocolVersion
    {
        LoginUdp_9  = 0x01,
        LoginUdp_10 = 0x02,

        LOGIN_ALL = LoginUdp_9 | LoginUdp_10
    }
}
