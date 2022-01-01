namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.DataFragment, useEncryption: true)]
    public class DataFragment : IProtocol
    {
        public ushort Sequence { get; set; }
        public ushort FragmentEnd { get; set; }
        public ushort CRC { get; set; } = 0;
        public byte[] Data { get; set; }
        public PacketOptions PacketOptions { get; private set; }

        public void Read(GamePacketReader reader, PacketOptions options)
        {
            PacketOptions = options;

            reader.ReadByte();
            Sequence = reader.ReadUShortBE();
            Data     = reader.ReadBytes(reader.BytesRemaining - 2);
            CRC      = reader.ReadUShortBE();
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
