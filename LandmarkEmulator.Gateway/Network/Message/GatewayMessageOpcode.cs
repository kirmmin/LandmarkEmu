namespace LandmarkEmulator.Gateway.Network.Message
{
    public enum GatewayMessageOpcode
    {
        LoginRequest                       = 0x01,
        LoginReply                         = 0x02,
        Logout                             = 0x03,
        ForceDisconnect                    = 0x04,
        TunnelPacketToExternalConnection   = 0x05,
        TunnelPacketFromExternalConnection = 0x06,
        ChannelIsRoutable                  = 0x07,
        ConnectionIsNotRoutable            = 0x08
    }
}
