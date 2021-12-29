namespace LandmarkEmulator.Shared.Network.Message
{
    public interface IWritable
    {
        void Write(GamePacketWriter writer);
    }
}
