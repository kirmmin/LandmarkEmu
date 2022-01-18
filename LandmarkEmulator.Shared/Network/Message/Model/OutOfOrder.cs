namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.OutOfOrder, useEncryption: true)]
    public class OutOfOrder : IProtocol
    {
        public byte CompressionFlag { get; set; } = 0;
        public ushort Sequence { get; set; }

        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
            if (!options.IsSubpacket)
                CompressionFlag = reader.ReadByte();
            Sequence = reader.ReadUShort();
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
            if (!options.IsSubpacket)
                writer.Write(CompressionFlag);
            writer.Write(Sequence);
        }
    }
}
