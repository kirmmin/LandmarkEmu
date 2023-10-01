using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.AbilityDefaultInfoRequest)]
    public class AbilityDefaultInfoRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            
        }
    }
}
