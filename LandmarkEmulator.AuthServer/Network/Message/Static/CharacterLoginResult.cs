namespace LandmarkEmulator.AuthServer.Network.Message.Static
{
    public enum CharacterLoginResult
    {
        NoResult            = 0,
        Success             = 1,
        AccountInUse        = 2,
        ServerNotFound      = 3,
        ServerLocked        = 4,
        CharacterLocked     = 5,
        WrongAccount        = 6,
        Exception           = 7,
        CharacterQueued     = 8,
        NotAllowed          = 9,
        ServerFeatureLocked = 10
    }
}
