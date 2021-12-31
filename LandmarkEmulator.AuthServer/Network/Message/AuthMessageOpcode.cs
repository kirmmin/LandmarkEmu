namespace LandmarkEmulator.AuthServer.Network.Message
{
    public enum AuthMessageOpcode
    {
        LoginRequest             = 0x01,
        LoginReply               = 0x02,
        CharacterSelectInfoReply = 0x0C,
        ServerListReply          = 0x0E,
    }
}
