using LandmarkEmulator.Shared.Network.Message;

namespace LandmarkEmulator.Shared.Network.Packets
{
    public abstract class GamePacket
    {
        public const ushort HeaderSize = 0; //sizeof(uint) + sizeof(ushort);

        /// <summary>
        /// Total size including the header and payload.
        /// </summary>
        public uint Size { get; protected set; }
        public GameMessageOpcode Opcode { get; protected set; }

        public byte[] Data { get; protected set; }
    }
}
