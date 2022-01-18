using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.LoginReply, ProtocolVersion.LOGIN_ALL)]
    public class LoginReply : IWritable
    {
        public bool LoggedIn { get; set; }
        public uint Status { get; set; }
        public uint Result { get; set; }
        public bool IsMember { get; set; }
        public bool IsInternal { get; set; }
        public string Namespace { get; set; } = "soe";
        public ulong AccountFeatures { get; set; } // KVP?
        public byte[] Payload { get; set; } // XML
        public ulong ErrorDetails { get; set; } // KVP?

        public void Write(GamePacketWriter writer)
        {
            writer.Write(LoggedIn);
            writer.Write(Status);
            writer.Write(Result);
            writer.Write(IsMember);
            writer.Write(IsInternal);
            writer.Write(Namespace);
            writer.Write(0ul); // TODO: Figure out how to write "byteswithlength" type.
            writer.Write(0ul); // TODO: Figure out how to write "byteswithlength" type.
            writer.Write(0ul); // TODO: Figure out how to write "byteswithlength" type.
        }
    }
}
