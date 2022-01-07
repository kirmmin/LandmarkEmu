namespace LandmarkEmulator.AuthServer.Network.Message
{
    public enum AuthMessageOpcode
    {
        LoginRequest               = 0x01,
        LoginReply                 = 0x02,
        CharacterCreateRequest     = 0x05,
        CharacterCreateReply       = 0x06,
        CharacterSelectInfoRequest = 0x0B,
        CharacterSelectInfoReply   = 0x0C,
        ServerListRequest          = 0x0D,
        ServerListReply            = 0x0E,
        TunnelPacketClientToServer = 0x10,
        TunnelPacketServerToClient = 0x11
    }
}
