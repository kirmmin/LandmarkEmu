using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.AuthServer.Network.Message.Model
{
    [AuthMessage(AuthMessageOpcode.ForceDisconnect, ProtocolVersion.LOGIN_ALL)]
    public class ForceDisconnect : IWritable
    {
        public uint Reason { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteLE(Reason);
        }
    }
}
