using LandmarkEmulator.Shared.Network.Message;
using System.IO;

namespace LandmarkEmulator.Shared.Network.Packets
{
    public class ClientGamePacket : GamePacket
    {
        public ClientGamePacket(byte[] data)
        {
            var reader = new GamePacketReader(data);

            Opcode = (GameMessageOpcode)reader.ReadUShort();
            Data = reader.ReadBytes(reader.BytesRemaining);
            Size = (uint)Data.Length + HeaderSize;
        }
    }
}
