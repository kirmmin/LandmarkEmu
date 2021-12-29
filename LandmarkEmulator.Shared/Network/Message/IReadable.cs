namespace LandmarkEmulator.Shared.Network.Message
{
    public interface IReadable
    {
        void Read(GamePacketReader reader);
    }
}
