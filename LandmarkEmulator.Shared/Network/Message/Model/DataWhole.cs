namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Data, useEncryption: true)]
    public class DataWhole : IProtocol
    {
        public ushort Sequence { get; set; }
        public ushort FragmentEnd { get; set; }
        public byte[] Data { get; set; }
        public ushort CRC { get; set; } = 0;
        public PacketOptions PacketOptions { get; private set; }

        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
            int dataEnd = options.IsSubpacket ? 0 : 2;

            if (options.Compression && !options.IsSubpacket)
                reader.ReadByte();

            Sequence = reader.ReadUShort();
            Data     = reader.ReadBytes((uint)(reader.BytesRemaining - dataEnd));

            if (!options.IsSubpacket)
                CRC = reader.ReadUShort();

            PacketOptions = options;
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
            if (options.Compression && !options.IsSubpacket)
                writer.Write((byte)0);

            writer.Write(Sequence);
            writer.WriteBytes(Data);
        }
    }
}
