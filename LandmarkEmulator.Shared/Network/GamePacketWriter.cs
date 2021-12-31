using NLog;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace LandmarkEmulator.Shared.Network
{
    public class GamePacketWriter
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private int currentBytePosition;
        private List<byte> stream;

        public GamePacketWriter(List<byte> output)
        {
            stream = output;
            ResetBits();
        }

        public void ResetBits()
        {
            if (currentBytePosition == 0)
                return;

            currentBytePosition = 0;
        }

        public void Write(byte value)
        {
            stream.Add(value);
        }

        public void Write(bool value)
        {
            Write(Convert.ToByte(value));
        }

        public void WriteBE(ushort value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[2]);
            BinaryPrimitives.WriteUInt16BigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteBE(short value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[2]);
            BinaryPrimitives.WriteInt16BigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteLE(uint value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteUInt32LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteBE(uint value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteUInt32BigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteBE(int value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteInt32BigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(float value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteSingleBigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteLE(ulong value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteUInt64LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteBE(ulong value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteUInt64BigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteBE(long value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteInt64BigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void WriteBE(double value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteDoubleBigEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteLE((uint)bytes.Length);
            foreach (byte c in bytes)
                stream.Add(c);
        }

        public void WriteNullTerminatedString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            foreach (byte c in bytes)
                stream.Add(c);

            stream.Add(0);
        }

        public void WriteBytes(byte[] data, uint length = 0u)
        {
            if (length != 0 && length != data.Length)
                throw new ArgumentException();

            foreach (byte value in data)
                Write(value);
        }
    }
}
