using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.ServerListReply, ProtocolVersion.LOGIN_ALL)]
    public class ServerListReply : IWritable, IReadable
    {
        public List<Server> Servers { get; set; } = new();

        public void Read(GamePacketReader reader)
        {
            var serverCount = reader.ReadUInt();
            for (int i = 0; i < serverCount; i++)
            {
                var server = new Server();
                server.Read(reader);
                Servers.Add(server);
            }
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Servers.Count);
            Servers.ForEach(i => i.Write(writer));
        }
    }
}
