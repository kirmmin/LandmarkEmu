namespace LandmarkEmulator.AuthServer.Network.Message
{
    public enum AuthMessageOpcode
    {
        LoginRequest               = 0x01,
        LoginReply                 = 0x02,
        Logout                     = 0x03,
        ForceDisconnect            = 0x04,
        CharacterCreateRequest     = 0x05,
        CharacterCreateReply       = 0x06,
        CharacterLoginRequest      = 0x07,
        CharacterLoginReply        = 0x08,
        CharacterDeleteRequest     = 0x09,
        CharacterDeleteReply       = 0x0A,
        CharacterSelectInfoRequest = 0x0B,
        CharacterSelectInfoReply   = 0x0C,
        ServerListRequest          = 0x0D,
        ServerListReply            = 0x0E,
        ServerUpdate               = 0x0F,
        TunnelPacketClientToServer = 0x10,
        TunnelPacketServerToClient = 0x11
    }
}
