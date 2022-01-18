namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Disconnect, useEncryption: true)]
    public class Disconnect : IProtocol
    {
        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
        }
    }
}
