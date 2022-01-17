using LandmarkEmulator.AuthServer.Network.Message.Model.Shared;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.ServerUpdate, ProtocolVersion.LOGIN_ALL)]
    public class ServerUpdate : IWritable
    {
        public Server Server { get; set; } = new();

        public void Write(GamePacketWriter writer)
        {
            Server.Write(writer);
        }
    }
}
