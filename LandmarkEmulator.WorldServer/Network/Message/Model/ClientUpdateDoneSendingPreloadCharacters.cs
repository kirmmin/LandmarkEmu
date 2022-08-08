using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.ClientUpdateDoneSendingPreloadCharacters)]
    public class ClientUpdateDoneSendingPreloadCharacters : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            writer.Write((byte)0);
        }
    }
}
