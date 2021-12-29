namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.DataFragment)]
    public class DataFragment : IReadable, IWritable
    {
        public ushort Sequence { get; set; }
        public ushort FragmentEnd { get; set; }
        public ushort CRC { get; set; } = 0;
        public byte[] Data { get; set; }

        public void Read(GamePacketReader reader)
        {
            reader.ReadByte();
            Sequence = reader.ReadUShort();
            Data     = reader.ReadBytes(reader.BytesRemaining - 2);
            CRC      = reader.ReadUShort();
        }

        public void Write(GamePacketWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
