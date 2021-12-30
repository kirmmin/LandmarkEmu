using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.Shared.Network;

namespace LandmarkEmulator.AuthServer.Network.Packets
{
    public class AuthPacket
    {
        public const ushort HeaderSize = 0; //sizeof(uint) + sizeof(ushort);

        /// <summary>
        /// Total size including the header and payload.
        /// </summary>
        public uint Size { get; protected set; }
        public AuthMessageOpcode Opcode { get; protected set; }

        public byte[] Data { get; protected set; }

        public AuthPacket(byte[] data)
        {
            var reader = new GamePacketReader(data);

            Opcode = (AuthMessageOpcode)reader.ReadByte();
            Data = reader.ReadBytes(reader.BytesRemaining);
            Size = (uint)Data.Length;
        }
    }
}
