using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterCreateReply, ProtocolVersion.LOGIN_ALL)]
    public class CharacterCreateReply : IWritable
    {
        public CharacterCreateResult Result { get; set; }
        public ulong CharacterId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE((uint)Result);
            writer.WriteLE(CharacterId);
        }
    }
}
