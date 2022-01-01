using LandmarkEmulator.Shared.Network.Message.Model;

namespace LandmarkEmulator.Shared.Network.Message
{
    public interface IProtocol
    {
        void Write(GamePacketWriter writer, PacketOptions options);
        void Read(GamePacketReader reader, PacketOptions options);
    }
}
