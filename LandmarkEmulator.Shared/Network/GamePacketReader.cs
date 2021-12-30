using NLog;
using System;
using System.Buffers.Binary;
using System.Text;

namespace LandmarkEmulator.Shared.Network
{
    public class GamePacketReader
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public uint BytesRemaining => (uint)(stream.Length - currentBytePosition);

        private int currentBytePosition;
        private byte[] stream;

        public GamePacketReader(byte[] input)
        {
            stream = input;
            ResetBits();
        }

        public void ResetBits()
        {
            if (currentBytePosition > 7)
                return;

            currentBytePosition = 0;
        }

        public byte ReadByte(uint bits = 8u)
        {
            if (bits > sizeof(byte) * 8)
                throw new ArgumentException();

            return GetDataBits(bits)[0];
        }

        public ushort ReadUShortBE(uint bits = 16u)
        {
            if (bits > sizeof(ushort) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadUInt16BigEndian(GetDataBits(bits));
        }

        public short ReadShortBE(uint bits = 16u)
        {
            if (bits > sizeof(short) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadInt16BigEndian(GetDataBits(bits));
        }

        public uint ReadUIntLE(uint bits = 32u)
        {
            if (bits > sizeof(uint) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadUInt32LittleEndian(GetDataBits(bits));
        }

        public uint ReadUIntBE(uint bits = 32u)
        {
            if (bits > sizeof(uint) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadUInt32BigEndian(GetDataBits(bits));
        }

        public int ReadIntBE(uint bits = 32u)
        {
            if (bits > sizeof(int) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadInt32BigEndian(GetDataBits(bits));
        }

        public unsafe float ReadSingleBE(uint bits = 32u)
        {
            if (bits > sizeof(float) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadSingleBigEndian(GetDataBits(bits));
        }

        public ulong ReadULongBE(uint bits = 64u)
        {
            if (bits > sizeof(ulong) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadUInt64BigEndian(GetDataBits(bits));
        }

        public long ReadLongBE(uint bits = 64u)
        {
            if (bits > sizeof(ulong) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadInt64BigEndian(GetDataBits(bits));
        }

        public unsafe double ReadDoubleBE(uint bits = 64u)
        {
            if (bits > sizeof(double) * 8)
                throw new ArgumentException();

            return BinaryPrimitives.ReadDoubleBigEndian(GetDataBits(bits));
        }

        public T ReadEnum<T>(uint bits = 64u) where T : Enum
        {
            if (bits > sizeof(ulong) * 8)
                throw new ArgumentException();

            return (T)Enum.ToObject(typeof(T), 0ul); // ReadBits(bits));
        }

        public byte[] ReadBytes(uint length)
        {
            byte[] data = new byte[length];
            for (uint i = 0u; i < length; i++)
                data[i] = ReadByte();

            return data;
        }

        public string ReadStringLE()
        {
            var length = ReadUIntLE();
            var value = ReadBytes(length);

            return Encoding.UTF8.GetString(value);
        }

        public string ReadStringBE()
        {
            var length = ReadUIntBE();
            var value = ReadBytes(length);

            return Encoding.UTF8.GetString(value);
        }

        public string ReadNullTerminatedString()
        {
            var data = new byte[BytesRemaining];
            Buffer.BlockCopy(stream, currentBytePosition, data, 0, data.Length);

            int nullterm = 0;
            while (nullterm < BytesRemaining && data[nullterm] != 0)
            {
                nullterm = nullterm + 1; // Don't include the null terminator as part of parse
            }

            string s = Encoding.UTF8.GetString(data, 0, nullterm);
            currentBytePosition += nullterm + 1; // Adjust byte position to nullterm + 1, so that bytePosition is at the byte after the null terminator
            return s;
        }

        public ReadOnlySpan<byte> GetDataBits(uint bits)
        {
            byte[] data = new byte[bits / 8];
            Buffer.BlockCopy(stream, currentBytePosition, data, 0, data.Length);

            currentBytePosition += (byte)bits / 8;
            return new ReadOnlySpan<byte>(data);
        }
    }
}
