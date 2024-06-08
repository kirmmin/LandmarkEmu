using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.WorldServer.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.WorldServer.Network.Packets
{
    public class ZonePacket
    {
        /// <summary>
        /// Total size including the header and payload.
        /// </summary>
        public uint Size { get; protected set; }
        public ZoneMessageOpcode Opcode { get; protected set; }
        public bool PrependSize { get; protected set; }

        public byte[] Data { get; protected set; }

        public ZonePacket(ZoneMessageOpcode opcode, byte[] data)
        {
            Opcode = opcode;

            var reader = new GamePacketReader(data);
            Data = reader.ReadBytes(reader.BytesRemaining);
            Size = (uint)Data.Length;
        }

        public ZonePacket(ZoneMessageOpcode opcode, IWritable message, bool prependSize = false)
        {
            Opcode = opcode;
            PrependSize = prependSize;

            List<byte> data = new();
            var writer = new GamePacketWriter(data);
            message.Write(writer);

            if (prependSize)
            {
                List<byte> prependData = new();
                var prependWriter = new GamePacketWriter(prependData);
                prependWriter.Write((uint)data.Count);
                prependData.AddRange(data);
                data = prependData;
            }

            Data = data.ToArray();
            Size = (uint)Data.Length;
        }
    }
}
