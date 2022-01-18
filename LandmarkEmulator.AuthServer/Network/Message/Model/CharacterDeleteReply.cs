using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterDeleteReply, ProtocolVersion.LOGIN_ALL)]
    public class CharacterDeleteReply : IWritable
    {
        public ulong CharacterId { get; set; }
        public uint Status { get; set; }
        public string Payload { get; set; } = "";

        public void Write(GamePacketWriter writer)
        {
            writer.Write(CharacterId);
            writer.Write(Status);
            writer.Write(Payload);
        }
    }
}
