using LandmarkEmulator.AuthServer.Network.Message.Static;
using LandmarkEmulator.Shared.Network;

namespace LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData
{
    [TunnelData(TunnelDataType.LoginInit)]
    public class LoginInit : ITunnelData
    {
        public uint GetSize()
        {
            return 0;
        }

        public void Read(GamePacketReader reader)
        {
        }

        public void Write(GamePacketWriter writer)
        {
        }
    }
}
