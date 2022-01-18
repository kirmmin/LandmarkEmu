using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.LoginRequest, ProtocolVersion.LOGIN_ALL)]
    public class LoginRequest : IReadable
    {
        public string SessionId { get; private set; }
        public string SystemFingerPrint { get; private set; }
        public uint Locale { get; private set; }
        public uint ThirdPartyAuthTicket { get; private set; }

        public void Read(GamePacketReader reader)
        {
            SessionId            = reader.ReadString();
            SystemFingerPrint    = reader.ReadString();
            Locale               = reader.ReadUInt();
            ThirdPartyAuthTicket = reader.ReadUInt();
        }
    }
}
