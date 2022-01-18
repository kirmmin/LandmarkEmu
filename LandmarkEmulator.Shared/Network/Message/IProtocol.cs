using LandmarkEmulator.Shared.Network;

namespace LandmarkEmulator.Shared.Network.Message
{
    public interface IProtocol
    {
        void Write(ProtocolPacketWriter writer, PacketOptions options);
        void Read(ProtocolPacketReader reader, PacketOptions options);
    }
}
