namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionRequest, useEncryption: false)]
    public class SessionRequest : IProtocol
    {
        public uint CRCLength { get; set; }
        public uint SessionId { get; set; }
        public uint UdpLength { get; set; }
        public string Protocol { get; set; }

        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
            CRCLength = reader.ReadUInt();
            SessionId = reader.ReadUInt();
            UdpLength = reader.ReadUInt();
            Protocol  = reader.ReadNullTerminatedString();
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
