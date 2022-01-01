namespace LandmarkEmulator.AuthServer.Network.Message
{
    public enum AuthMessageOpcode
    {
        LoginRequest               = 0x01,
        LoginReply                 = 0x02,
        CharacterSelectInfoRequest = 0x0B,
        CharacterSelectInfoReply   = 0x0C,
        ServerListRequest          = 0x0D,
        ServerListReply            = 0x0E
    }
}
