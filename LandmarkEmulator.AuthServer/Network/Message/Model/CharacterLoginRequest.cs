using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterLoginRequest, ProtocolVersion.LoginUdp_9)]
    public class CharacterLoginRequest : IReadable
    {
        public ulong CharacterId { get; set; }
        public ulong ServerId { get; set; }
        public uint Unknown0 { get; set; }
        public string Locale { get; set; }
        public ulong Unknown2 { get; set; }

        public void Read(GamePacketReader reader)
        {
            CharacterId = reader.ReadULongLE();
            ServerId = reader.ReadULongLE();
            Unknown0 = reader.ReadUIntLE();
            Locale = reader.ReadStringLE();
            Unknown2 = reader.ReadULongLE();
        }
    }
}
