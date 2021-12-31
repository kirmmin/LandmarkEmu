using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.LoginReply, MessageDirection.Server)]
    public class LoginReply : IWritable
    {
        public bool LoggedIn { get; set; }
        public uint Status { get; set; }
        public bool IsMember { get; set; }
        public bool IsInternal { get; set; }
        public string Namespace { get; set; } = "";
        public byte[] Payload { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(LoggedIn);
            writer.WriteLE(Status);
            writer.Write(IsMember);
            writer.Write(IsInternal);
            writer.Write(Namespace);
            writer.WriteLE(0u); // TODO: Figure out how to write "byteswithlength" type.
        }
    }
}
