using LandmarkEmulator.Shared.Network.Cryptography;
using NLog;
using System;
using System.Collections.Generic;

namespace LandmarkEmulator.Shared.Network
{
    public class DataStreamOutput : DataStreamBase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public delegate void DataEvent(ushort sequence, DataPacket packet);
        /// <summary>
        /// Raised on data processing complete, and packet available.
        /// </summary>
        public event DataEvent OnData;

        private uint _fragmentSize;
        private Arc4Provider arc4Provider;

        public DataStreamOutput(GameSession session)
            : base (session)
        {
            arc4Provider = new Arc4Provider(RC4Key);
        }

        public void PackData(byte[] data)
        {
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

            // TODO: Write and queue DataFragments when size exceeds _fragmentSize
        }

        public void SetFragmentSize(uint fragmentSize)
        {
            _fragmentSize = fragmentSize;
        }
    }
}
