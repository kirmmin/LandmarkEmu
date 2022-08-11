namespace LandmarkEmulator.Shared.Network.Message
{
    public class PacketOptions
    {
        public bool Compression { get; set; }
        public bool IsSubpacket { get; set; }
        public bool WasOutOfOrder { get; set; }
        public ushort Sequence { get; set; }
        public bool IsPriority { get; set; } = false;
    }
}
