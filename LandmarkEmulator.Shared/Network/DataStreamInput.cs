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

        public delegate void OutOfOrderEvent(ushort sequence);
        /// <summary>
        /// Raised on data processing complete, and packet available.
        /// </summary>
        public event OutOfOrderEvent OnOutOfOrder;

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
                if (OnOutOfOrder != null)
                    OnOutOfOrder(sequence);
                return;
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
                _session?.EnqueueProtocolMessage(new Ack
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
            var reader = new ProtocolPacketReader(dataPacket.Data);
            uint totalSize = reader.ReadUInt();
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
                var reader = new ProtocolPacketReader(data.ToArray().AsSpan().Slice(2).ToArray());
                while (reader.BytesRemaining != 0)
                {
                    newData.Add(ReadPacket(reader));
                }
            }
            else
                newData.Add(data.ToArray());

            return newData;
        }

        private byte[] ReadPacket(ProtocolPacketReader reader)
        {
            uint nextLength = ReadLength(reader);
            if (nextLength > reader.BytesRemaining)
                nextLength = (byte)reader.BytesRemaining;
            return reader.ReadBytes(nextLength);
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

        private void DecryptData(List<byte[]> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                byte[] piece = data[i];
                
                var reader = new ProtocolPacketReader(piece);
                byte[] parsedData = piece;
                if (UsingEncryption) // Do we turn off encryption, ever?
                {
                    // sometimes there's an extra 0x00 byte in the beginning that trips up the RC4
                    if (piece.Length > 1 && reader.ReadUShort() == 0)
                    {
                        parsedData = new Span<byte>(piece, 1, piece.Length - 1).ToArray();
                        arc4Provider.Decrypt(parsedData);
                    }
                    else
                        arc4Provider.Decrypt(parsedData);
                }
                OnData(parsedData); // Emit the parsed data packet to subscribers
            }
        }

        public void ResetEncryption()
        {
            arc4Provider = new Arc4Provider(RC4Key);
        }

        public void SetEncryptionKey(string key)
        {
            arc4Provider = new Arc4Provider(Convert.FromBase64String(key));
        }
    }
}
