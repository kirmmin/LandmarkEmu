using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.Shared.Network.Packets
{
    public class ProtocolPacket
    {
        /// <summary>
        /// Total size including the header and payload.
        /// </summary>
        public uint Size { get; protected set; }
        public ProtocolMessageOpcode Opcode { get; protected set; }
        public byte[] Data { get; protected set; }
        public bool UseEncryption { get; protected set; }

        public ProtocolPacket(byte[] data)
        {
            var reader = new GamePacketReader(data);

            Opcode = (ProtocolMessageOpcode)reader.ReadUShort();
            Data   = reader.ReadBytes(reader.BytesRemaining);
            Size   = (uint)Data.Length;
        }

        public ProtocolPacket(ProtocolMessageOpcode opcode, IWritable message, bool useEncryption)
        {
            Opcode = opcode;
            UseEncryption = useEncryption;

            List<byte> data = new();
            var writer = new GamePacketWriter(data);
            message.Write(writer);

            Data = data.ToArray();
        }
    }
}
