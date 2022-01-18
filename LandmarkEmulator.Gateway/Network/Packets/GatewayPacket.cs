using LandmarkEmulator.Gateway.Network.Message;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.Gateway.Network.Packets
{
    public class GatewayPacket
    {
        /// <summary>
        /// Total size including the header and payload.
        /// </summary>
        public uint Size { get; protected set; }
        public GatewayMessageOpcode Opcode { get; protected set; }

        public byte[] Data { get; protected set; }

        public GatewayPacket(byte[] data)
        {
            var reader = new GamePacketReader(data);

            Opcode = (GatewayMessageOpcode)reader.ReadByte();
            Data = reader.ReadBytes(reader.BytesRemaining);
            Size = (uint)Data.Length;
        }

        public GatewayPacket(GatewayMessageOpcode opcode, IWritable message)
        {
            Opcode = opcode;

            List<byte> data = new();
            var writer = new GamePacketWriter(data);
            message.Write(writer);

            Data = data.ToArray();
            Size = (uint)Data.Length;
        }
    }
}
