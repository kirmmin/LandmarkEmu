using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.WorldServer.Network.Message.Model
{
    [ZoneMessage(ZoneMessageOpcode.AbilityCasterInfoRequest)]
    public class AbilityCasterInfoRequest : IReadable
    {
        public uint Unknown0 { get; set; }

        public void Read(GamePacketReader reader)
        {
            Unknown0 = reader.ReadUInt();
        }
    }
}
