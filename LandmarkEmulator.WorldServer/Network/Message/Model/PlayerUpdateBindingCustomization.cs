using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.PlayerUpdateBindingCustomization)]
    public class PlayerUpdateBindingCustomization : IReadable
    {
        public void Read(GamePacketReader reader)
        {
        }
    }
}
