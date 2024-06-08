using NLog;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Numerics;
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

        public void WriteLE(ushort value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[2]);
            BinaryPrimitives.WriteUInt16LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(uint value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteUInt32LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(int value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteInt32LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(float value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteSingleLittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(ulong value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteUInt64LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(long value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteInt64LittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(double value)
        {
            Span<byte> spanValue = new Span<byte>(new byte[8]);
            BinaryPrimitives.WriteDoubleLittleEndian(spanValue, value);

            foreach (byte i in spanValue)
                stream.Add(i);
        }

        public void Write(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            Write((uint)bytes.Length);
            foreach (byte c in bytes)
                stream.Add(c);
        }

        public void Write(Vector4 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
            Write(value.W);
        }

        public void WriteLongString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            Write((ulong)bytes.Length);
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

        public void WriteUIntWith2BitLength(int value)
        {
            value = (int)MathF.Round(value);
            value = value << 2;
            var n = 0;
            if (value > 0xFFFFFF)
            {
                n = 3;
            }
            else if (value > 0xFFFF)
            {
                n = 2;
            }
            else if (value > 0xFF)
            {
                n = 1;
            }
            value |= n;
            var data = new Span<byte>(new byte[4]);
            BinaryPrimitives.WriteInt32LittleEndian(data, value);
            foreach (byte d in data.Slice(0, n + 1))
                stream.Add(d);
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
