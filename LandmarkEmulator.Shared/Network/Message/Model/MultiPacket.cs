using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using LandmarkEmulator.Shared.Network.Packets;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.MultiPacket, useEncryption: true)]
    public class MultiPacket : IProtocol
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public List<ProtocolPacket> Packets { get; } = new();

        public void Read(ProtocolPacketReader reader, PacketOptions options)
        {
            byte[] data = reader.GetRemainingData();

            if (options.Compression)
            {
                byte compression = reader.ReadByte();
                if (compression > 0 && data[1] == 0x78 && data[2] == 0x9C)
                {
                    data = reader.ReadBytes((uint)(data.Length - 3));
                    reader = new ProtocolPacketReader(ZLibDotnetDecompress(data));
                    log.Trace($"Decompressed: {BitConverter.ToString(reader.GetRemainingData())}");
                }
            }

            while (reader.BytesRemaining > 2)
            {
                try
                {
                    ReadPacket(reader, options);
                }
                catch (Exception ex)
                {
                    log.Error($"{BitConverter.ToString(data)} : {BitConverter.ToString(reader.GetRemainingData())}");
                }
            }
        }

        private void ReadPacket(ProtocolPacketReader reader, PacketOptions options)
        {
            uint nextLength = ReadLength(reader);
            if (nextLength > reader.BytesRemaining)
                nextLength = (byte)reader.BytesRemaining;

            Packets.Add(new ProtocolPacket(reader.ReadBytes(nextLength),
                new PacketOptions
                {
                    IsSubpacket = true,
                    Compression = options.Compression
                }));
        }

        private uint ReadLength(ProtocolPacketReader reader)
        {
            var data = reader.GetRemainingData();
            uint dataLength = reader.ReadByte();
            int n = 1;
            if (dataLength == 0xFF)
            {
                if (data[0 + 1] == 0xFF && data[0 + 2] == 0xFF)
                {
                    // Move the current position 2 bytes forward,
                    // because length is placed 3 bytes (we read 1 byte at the beginning of method) ahead of beginning
                    reader.ReadUShort();
                    dataLength = reader.ReadUInt();
                    n = 7;
                }
                else
                {
                    dataLength = reader.ReadUShort();
                    n = 3;
                }
            }

            return dataLength;
        }

        public void Write(ProtocolPacketWriter writer, PacketOptions options)
        {
            throw new System.NotImplementedException();
        }

        public static byte[] ZLibDotnetDecompress(byte[] data)
        {
            MemoryStream compressed = new MemoryStream(data);
            MemoryStream decompressed = new MemoryStream();
            InflaterInputStream inputStream = new InflaterInputStream(compressed);
            inputStream.CopyTo(decompressed);
            return decompressed.ToArray();
        }
    }
}
