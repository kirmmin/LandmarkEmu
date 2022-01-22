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

        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
            SessionId = reader.ReadUInt();
            CRCSeed = reader.ReadUInt();
            CRCLength = reader.ReadByte();
            Compression = reader.ReadUShort();
            UdpLength = reader.ReadUInt();
            Unknown0 = reader.ReadUInt();
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
            writer.Write(SessionId);
            writer.Write(CRCSeed);
            writer.Write(CRCLength);
            writer.Write(Compression);
            writer.Write(UdpLength);
            writer.Write(Unknown0);
        }
    }
}
