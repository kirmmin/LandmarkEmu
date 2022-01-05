using LandmarkEmulator.Shared.Network.Cryptography;
using LandmarkEmulator.Shared.Network.Message.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LandmarkEmulator.Shared.Network
{
    public class DataStreamInput : DataStreamBase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public delegate void DataEvent(byte[] message);
        /// <summary>
        /// Raised on data processing complete, and packet available.
        /// </summary>
        public event DataEvent OnData;

        private int _lastProcessedFragment = -1;
        private Arc4Provider arc4Provider;

        public DataStreamInput(GameSession session)
            : base (session)
        {
            arc4Provider = new Arc4Provider(RC4Key);
        }

        public void ProcessDataFragment(DataWhole dataWhole)
        {
            ReadyDataForProcessing(dataWhole.Sequence, dataWhole.Data, false);
        }

        public void ProcessDataFragment(DataFragment dataFragment)
        {
            ReadyDataForProcessing(dataFragment.Sequence, dataFragment.Data, true);
        }

        private void ReadyDataForProcessing(ushort sequence, byte[] data, bool isFragment)
        {
            if (NextSequence == null)
                NextSequence = sequence;

            if (sequence > NextSequence)
            {
                log.Warn($"Sequence out of order, expected {NextSequence} but received {sequence}");
                _session.OnDisconnect();

                // TODO: Handle out of order packets
                // this.emit("outoforder", null, this._nextSequence, sequence);

                throw new InvalidOperationException();
            }

            int lastAck = LastAck ?? -1;
            ushort ack = sequence;
            for (ushort i = 1; i <= ushort.MaxValue; i++)
            {
                ushort j = (ushort)((lastAck + i) & 0xFFFF);
                if (DataPackets[j] != null)
                    ack = j;
                else
                    break;
            }

            if (ack > lastAck)
            {
                LastAck = ack;
                _session.EnqueueProtocolMessage(new Ack
                {
                    Sequence = ack
                }, new Message.PacketOptions());
            }

            DataPackets[sequence] = new DataPacket(data, isFragment);

            NextSequence = (ushort)((LastAck + 1) & 0xFFFF);

            ParseDataFragments();
        }

        private void ParseDataFragments()
        {
            var nextFragment = (_lastProcessedFragment + 1) & 0xFFFF;
            List<byte[]> finishedData = new();

            if (DataPackets[nextFragment] == null)
                return;

            var dataPacket = DataPackets[nextFragment];

            if (!dataPacket.IsFragment)
            {
                _lastProcessedFragment = nextFragment;
                finishedData = ParseChannelData(dataPacket.Data.ToList());
                DecryptData(finishedData);
                return;
            }
            
            bool dataReady = false;
            var reader = new GamePacketReader(dataPacket.Data);
            int totalSize = (int)reader.ReadUIntBE();
            int dataSize = dataPacket.Data.Length - 4;

            List<byte> data = new();
            for (int i = 4; i < dataPacket.Data.Length; i++)
                data.Add(dataPacket.Data[i]);

            List<ushort> fragmentIndices = new();
            for (int i = 1; i <= ushort.MaxValue; i++)
            {
                ushort j = (ushort)((nextFragment + i) % 0xFFFF);
                var fragment = DataPackets[j];
                if (fragment != null)
                {
                    fragmentIndices.Add(j);
                    for (int m = 0; m < fragment.Data.Length; m++)
                        data.Add(fragment.Data[m]);
                    dataSize += fragment.Data.Length;

                    if (dataSize > totalSize)
                        throw new InvalidPacketValueException("DataSize exceeds TotalSize. We've got a problem!");

                    if (dataSize == totalSize)
                    {
                        for (int k = 0; k < fragmentIndices.Count; k++)
                            DataPackets[fragmentIndices[k]] = null;
                        _lastProcessedFragment = j;
                        dataReady = true;
                        finishedData = ParseChannelData(data);
                        break;
                    }
                }
                else
                    break;
            }

            if (dataReady)
                DecryptData(finishedData);
        }

        private List<byte[]> ParseChannelData(List<byte> data)
        {
            var newData = new List<byte[]>();
            if (data[0] == 0x00 && data[1] == 0x19)
            {
                var reader = new GamePacketReader(data.ToArray().AsSpan().Slice(2).ToArray());
                while (reader.BytesRemaining != 0)
                {
                    newData.Add(ReadPacket(reader));
                }
            }
            else
                newData.Add(data.ToArray());

            return newData;
        }

        private byte[] ReadPacket(GamePacketReader reader)
        {
            byte nextLength = reader.ReadByte(); // TODO: Calculate length of packet appropriately.
            return reader.ReadBytes(nextLength);
        }

        private void DecryptData(List<byte[]> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                byte[] piece = data[i];
                if (true) // Do we turn off encryption, ever?
                {
                    // sometimes there's an extra 0x00 byte in the beginning that trips up the RC4
                    var reader = new GamePacketReader(piece);
                    byte[] parsedData = piece;
                    if (piece.Length > 1 && reader.ReadUShortBE() == 0)
                    {
                        parsedData = new Span<byte>(piece, 1, piece.Length - 1).ToArray();
                        arc4Provider.Decrypt(parsedData);
                    }
                    else
                        arc4Provider.Decrypt(parsedData);
                    OnData(parsedData); // Emit the parsed data packet to subscribers
                }
            }
        }
    }
}
