using LandmarkEmulator.Shared.Network;

namespace LandmarkEmulator.AuthServer.Network.Message
{
    public interface ITunnelData
    {
        void Read(GamePacketReader reader);
        void Write(GamePacketWriter writer);
        uint GetSize();
    }
}
