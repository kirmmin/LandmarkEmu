using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.CharacterLoginReply, ProtocolVersion.LOGIN_ALL)]
    public class CharacterLoginReply : IWritable, IReadable
    {
        public class ServerInfo : IWritable, IReadable, ISize
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

            public void Read(GamePacketReader reader)
            {
                reader.ReadUInt(); // Size
                ServerAddress = reader.ReadString();
                ServerTicket = reader.ReadString();

                uint keyCount = reader.ReadUInt();
                EncryptionKey = new byte[keyCount];
                for (int i = 0; i < keyCount; i++)
                    EncryptionKey[i] = reader.ReadByte();

                CharacterId = reader.ReadULong();
                Guid = reader.ReadULong();
                AccountName = reader.ReadString();
                CharacterName = reader.ReadString();
                Unknown0 = reader.ReadString();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(GetSize());
                writer.Write(ServerAddress);
                writer.Write(ServerTicket);
                writer.Write((uint)EncryptionKey.Length);
                foreach (byte key in EncryptionKey)
                    writer.Write(key);
                writer.Write(CharacterId);
                writer.Write(Guid);
                writer.Write(AccountName);
                writer.Write(CharacterName);
                writer.Write(Unknown0);
            }
        }

        public ulong CharacterId { get; set; }
        public ulong ServerId { get; set; }
        public CharacterLoginResult Result { get; set; }
        public ServerInfo Server { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            CharacterId = reader.ReadULong();
            ServerId = reader.ReadULong();
            Result = (CharacterLoginResult)reader.ReadUInt();
            Server.Read(reader);
        }
        
        public void Write(GamePacketWriter writer)
        {
            writer.Write(CharacterId);
            writer.Write(ServerId);
            writer.Write((uint)Result);
            Server.Write(writer);
        }
    }
}
