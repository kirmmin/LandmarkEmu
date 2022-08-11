namespace LandmarkEmulator.Shared.Network.Message
{
    public enum ProtocolMessageOpcode
    {
        SessionRequest   = 0x01,
        SessionReply     = 0x02,
        MultiPacket      = 0x03,
        Disconnect       = 0x05,
        Ping             = 0x06,
        NetStatusRequest = 0x07,
        NetStatusReply   = 0x08,
        Data             = 0x09,
        DataFragment     = 0x0D,
        OutOfOrder       = 0x11,
        Ack              = 0x15,
        FatalError       = 0x1D,
        FatalErrorReply  = 0x1E
    }
}
