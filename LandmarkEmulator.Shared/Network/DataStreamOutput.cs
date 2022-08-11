using LandmarkEmulator.Shared.Network.Cryptography;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandmarkEmulator.Shared.Network
{
    public class DataStreamOutput : DataStreamBase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public delegate void DataEvent(ushort sequence, DataPacket packet, bool wasOutOfOrder = false);
        /// <summary>
        /// Raised on data processing complete, and packet available.
        /// </summary>
        public event DataEvent OnData;

        public bool MissingAcks => LastAck != null && DataPackets[(int)LastAck] != null;

        private uint _fragmentSize;
        private Arc4Provider arc4Provider;

        public DataStreamOutput(GameSession session)
            : base (session)
        {
            arc4Provider = new Arc4Provider(RC4Key);
        }

        public void PackData(byte[] data)
        {
            if (UsingEncryption)
                arc4Provider.Encrypt(data);

            if (data[0] == 0)
            {
                List<byte> tmp = new List<byte> { 0 };
                tmp.AddRange(data);
                data = tmp.ToArray();
            }

            if (data.Length <= _fragmentSize)
            {
                if (NextSequence == null)
                    NextSequence = 0;
                else
                    NextSequence++;

                var dataPacket = new DataPacket(data, false);
                DataPackets[(int)NextSequence] = dataPacket;
                OnData((ushort)NextSequence, dataPacket);
                return;
            }

            List<byte> dataFragmentsCombined = new List<byte>();
            var writer = new ProtocolPacketWriter(dataFragmentsCombined);
            writer.Write((uint)data.Length);
            dataFragmentsCombined.AddRange(data);

            Span<byte> spanData = dataFragmentsCombined.ToArray().AsSpan();
            //log.Trace($"DataFragment total size is {spanData.Length}");
            for (var i = 0; i < spanData.Length; i += (int)_fragmentSize)
            {
                if (NextSequence == null)
                    NextSequence = 0;
                else
                    NextSequence++;

                int nextSize = i +(int)_fragmentSize > spanData.Length ? spanData.Length - i : (int)_fragmentSize;
                spanData = dataFragmentsCombined.ToArray().AsSpan();
                var nextSlice = spanData.Slice(i, nextSize).ToArray();
                var dataPacket = new DataPacket(nextSlice, true);
                DataPackets[(int)NextSequence] = dataPacket;
                OnData((ushort)NextSequence, dataPacket);
                //log.Trace($"DataFragment size is {nextSize}");
            }

            // TODO: Write and queue DataFragments when size exceeds _fragmentSize
        }

        public void SetFragmentSize(uint fragmentSize)
        {
            _fragmentSize = fragmentSize;
        }

        /// <summary>
        /// Used to handle <see cref="Ack"/> packet from Client.
        /// </summary>
        public void HandleAck(ushort sequence)
        {
            if (!LastAck.HasValue)
                LastAck = 0;

            while (LastAck < sequence)
            {
                if (DataPackets[(int)LastAck] != null)
                    DataPackets[(int)LastAck] = null;
                LastAck++;
            }
        }

        /// <summary>
        /// Used to handle resending of data to the client when it sends an out of order packet
        /// </summary>
        public void ResendData(ushort sequence)
        {
            var start = (int)(LastAck);
            for (var i = start; i <= sequence; i++)
            {
                if (DataPackets[i] != null)
                    OnData((ushort)i, DataPackets[i], wasOutOfOrder: true);
                else
                    throw new InvalidOperationException("Cache error, could not resend data!");
            }
        }

        /// <summary>
        /// Used to handle resending of data to the client when it sends an out of order packet
        /// </summary>
        public void ResendDataOnPing()
        {
            var start = (int)(LastAck);
            for (var i = start; i <= ushort.MaxValue; i++)
            {
                if (DataPackets[i] == null)
                    break;

                OnData((ushort)i, DataPackets[i], wasOutOfOrder: true);
            }
        }
    }
}
