namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionReply, useEncryption: true)]
    public class SessionReply : IProtocol
    {
        public uint SessionId { get; set; }
        public uint CRCSeed { get; set; }
        public byte CRCLength { get; set; }
        public ushort Compression { get; set; }
        public uint UdpLength { get; set; }
        public uint Unknown0 { get; set; } = 3;

        public void Read(GamePacketReader reader, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
            writer.WriteBE(SessionId);
            writer.WriteBE(CRCSeed);
            writer.Write(CRCLength);
            writer.WriteBE(Compression);
            writer.WriteBE(UdpLength);
            writer.WriteBE(Unknown0);
        }
    }
}
