namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Ping, useEncryption: true)]
    public class Ping : IProtocol
    {
        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
        }
    }
}
