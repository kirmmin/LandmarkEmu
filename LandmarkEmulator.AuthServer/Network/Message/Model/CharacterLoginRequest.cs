using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterLoginRequest, ProtocolVersion.LOGIN_ALL)]
    public class CharacterLoginRequest : IReadable
    {
        public ulong CharacterId { get; set; }
        public ulong ServerId { get; set; }
        public uint Unknown0 { get; set; }
        public string Locale { get; set; }
        public ulong Unknown2 { get; set; }

        public void Read(GamePacketReader reader)
        {
            CharacterId = reader.ReadULong();
            ServerId = reader.ReadULong();
            Unknown0 = reader.ReadUInt();
            Locale = reader.ReadString();
            Unknown2 = reader.ReadULong();
        }
    }
}
