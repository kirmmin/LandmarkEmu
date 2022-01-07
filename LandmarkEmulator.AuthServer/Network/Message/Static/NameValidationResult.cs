namespace LandmarkEmulator.AuthServer.Network.Message.Static
{
    public enum NameValidationResult
    {
        Fail            = 0,
        Success         = 1,
        NameTaken       = 2,
        NameInvalid     = 3,
        NameNaughty     = 4,
        NameUnavailable = 5
    }
}
