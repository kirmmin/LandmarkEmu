using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.Shared.Network.Message.Model;
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
        public PacketOptions PacketOptions { get; set; }
        public bool Ignore { get; set; } = false;

        public ProtocolPacket(byte[] data, PacketOptions options = null)
        {
            var reader = new ProtocolPacketReader(data);

            Opcode        = (ProtocolMessageOpcode)reader.ReadUShort();
            Data          = reader.ReadBytes((uint)reader.BytesRemaining);
            Size          = (uint)Data.Length;
            PacketOptions = options;
        }

        public ProtocolPacket(ProtocolMessageOpcode opcode, IProtocol message, bool useEncryption, PacketOptions options)
        {
            Opcode = opcode;
            UseEncryption = useEncryption;

            List<byte> data = new();
            var writer = new ProtocolPacketWriter(data);
            message.Write(writer, options);

            Data = data.ToArray();
        }
    }
}
