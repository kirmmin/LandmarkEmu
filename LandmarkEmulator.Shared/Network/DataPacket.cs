namespace LandmarkEmulator.Shared.Network
{
    public class DataPacket
    {
        public byte[] Data { get; }
        public bool IsFragment { get; }

        public DataPacket(byte[] data, bool isFragment)
        {
            Data = data;
            IsFragment = isFragment;
        }
    }
}
