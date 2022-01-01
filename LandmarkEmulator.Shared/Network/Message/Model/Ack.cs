namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Ack, useEncryption: true)]
    public class Ack : IProtocol
    {
        public byte CompressionFlag { get; set; } = 0;
        public ushort Sequence { get; set; }

        public void Read(GamePacketReader reader, PacketOptions options)
        {
            CompressionFlag = reader.ReadByte();
            Sequence = reader.ReadUShortBE();
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
            writer.Write(CompressionFlag);
            writer.WriteBE(Sequence);
        }
    }
}
