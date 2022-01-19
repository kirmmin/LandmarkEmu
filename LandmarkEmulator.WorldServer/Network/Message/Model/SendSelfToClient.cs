using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.SendSelfToClient)]
    public class SendSelfToClient : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            
        }
    }
}
