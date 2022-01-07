namespace LandmarkEmulator.AuthServer.Network.Message.Static
{
    public enum TunnelDataType
    {
        NameValidationRequest = 0x01,
        NameValidationReply   = 0x02,
        ServerQueue           = 0x04,
        LoginInit             = 0x06,
        /// <summary>
        /// Presumed to be a Login MOTD or an Error String.
        /// </summary>
        Unknown7              = 0x07,
        ClaimData             = 0x09,
        ArtData               = 0x0A
    }
}
