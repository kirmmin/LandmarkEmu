namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Data, useEncryption: true)]
    public class DataWhole : IReadable, IWritable
    {
        public ushort Sequence { get; set; }
        public ushort FragmentEnd { get; set; }
        public byte[] Data { get; set; }
        public ushort CRC { get; set; } = 0;

        public void Read(GamePacketReader reader)
        {
            reader.ReadByte();
            Sequence = reader.ReadUShortBE();
            Data     = reader.ReadBytes(reader.BytesRemaining - 2);
            CRC      = reader.ReadUShortBE();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write((byte)0); // We're using compression
            writer.WriteBE(Sequence);
            writer.WriteBytes(Data);
        }
    }
}
