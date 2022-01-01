namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionRequest, useEncryption: false)]
    public class SessionRequest : IProtocol
    {
        public uint CRCLength { get; set; }
        public uint SessionId { get; set; }
        public uint UdpLength { get; set; }
        public string Protocol { get; set; }

        public void Read(GamePacketReader reader, PacketOptions options)
        {
            CRCLength = reader.ReadUIntBE();
            SessionId = reader.ReadUIntBE();
            UdpLength = reader.ReadUIntBE();
            Protocol  = reader.ReadNullTerminatedString();
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}
