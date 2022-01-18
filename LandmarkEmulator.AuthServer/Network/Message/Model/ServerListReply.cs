using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.ServerListReply, ProtocolVersion.LOGIN_ALL)]
    public class ServerListReply : IWritable
    {
        public List<Server> Servers { get; set; } = new();

        public void Write(GamePacketWriter writer)
        {
            writer.Write((uint)Servers.Count);
            Servers.ForEach(i => i.Write(writer));
        }
    }
}
