using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ZoneDoneSendingInitialData)]
    public class ZoneDoneSendingInitialData : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            
        }
    }
}
