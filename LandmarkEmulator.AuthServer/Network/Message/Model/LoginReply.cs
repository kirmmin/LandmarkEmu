using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.LoginReply, ProtocolVersion.LoginUdp_10)]
    public class LoginReply : IWritable, IReadable
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

        public void Read(GamePacketReader reader)
        {
            LoggedIn = reader.ReadBool();
            Status = reader.ReadUInt();
            Result = reader.ReadUInt();
            IsMember = reader.ReadBool();
            IsInternal = reader.ReadBool();
            Namespace = reader.ReadString();
        }

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

    [AuthMessage(AuthMessageOpcode.LoginReply, ProtocolVersion.LoginUdp_9)]
    public class LoginReply_9 : IWritable, IReadable
    {
        public bool LoggedIn { get; set; }
        public uint Status { get; set; }
        public bool IsMember { get; set; }
        public bool IsInternal { get; set; }
        public string Namespace { get; set; } = "soe";
        public byte[] Payload { get; set; } // XML

        public void Read(GamePacketReader reader)
        {
            LoggedIn = reader.ReadBool();
            Status = reader.ReadUInt();
            IsMember = reader.ReadBool();
            IsInternal = reader.ReadBool();
            Namespace = reader.ReadString();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(LoggedIn);
            writer.Write(Status);
            writer.Write(IsMember);
            writer.Write(IsInternal);
            writer.Write(Namespace);
            writer.Write(0u); // TODO: Figure out how to write "byteswithlength" type.
        }
    }
}
