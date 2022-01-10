namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Disconnect, useEncryption: true)]
    public class Disconnect : IProtocol
    {
        public void Read(GamePacketReader reader, PacketOptions options)
        {
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
        }
    }
}
