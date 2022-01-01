namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Ping, useEncryption: true)]
    public class Ping : IProtocol
    {
        public void Read(GamePacketReader reader, PacketOptions options)
        {
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
        }
    }
}
