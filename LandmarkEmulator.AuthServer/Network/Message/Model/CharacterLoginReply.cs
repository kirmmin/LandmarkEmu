using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterLoginReply, ProtocolVersion.LoginUdp_9)]
    public class CharacterLoginReply : IWritable
    {
        public class ServerInfo : IWritable, ISize
        {
            public string ServerAddress { get; set; } = "";
            public string ServerTicket { get; set; } = "";
            public byte[] EncryptionKey { get; set; } = new byte[0];
            public ulong CharacterId { get; set; }
            public ulong Guid { get; set; }
            public string AccountName { get; set; } = "";
            public string CharacterName { get; set; } = "";
            public string Unknown0 { get; set; } = "";

            public uint GetSize()
            {
                return (uint)(4u + ServerAddress.Length +
                    4u + ServerTicket.Length +
                    4u + EncryptionKey.Length +
                    8u +
                    8u +
                    4u + AccountName.Length +
                    4u + CharacterName.Length +
                    4u + Unknown0.Length);
            }

            public void Write(GamePacketWriter writer)
            {
                writer.WriteLE(GetSize());
                writer.WriteLE(ServerAddress);
                writer.WriteLE(ServerTicket);
                writer.WriteLE((uint)EncryptionKey.Length);
                foreach (byte key in EncryptionKey)
                    writer.Write(key);
                writer.WriteLE(CharacterId);
                writer.WriteLE(Guid);
                writer.WriteLE(AccountName);
                writer.WriteLE(CharacterName);
                writer.WriteLE(Unknown0);
            }
        }

        public ulong CharacterId { get; set; }
        public ulong ServerId { get; set; }
        public CharacterLoginResult Result { get; set; }
        public ServerInfo Server { get; set; } = new();
        
        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE(CharacterId);
            writer.WriteLE(ServerId);
            writer.WriteLE((uint)Result);
            Server.Write(writer);
        }
    }
}
