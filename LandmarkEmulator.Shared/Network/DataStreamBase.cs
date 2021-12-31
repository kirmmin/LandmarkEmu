using System;
using System.Collections.Generic;
using System.Text;

namespace LandmarkEmulator.Shared.Network
{
    public class DataStreamBase
    {
        protected GameSession _session;

        /// <summary>
        /// Used to track Ack packet sequences.
        /// </summary>
        public ushort? LastAck { get; protected set; } = null;

        /// <summary>
        /// Used to track data packet sequences.
        /// </summary>
        public ushort? NextSequence { get; protected set; } = null;

        public byte[] RC4Key { get; private set; }

        protected readonly DataPacket[] DataPackets = new DataPacket[ushort.MaxValue];

        public DataStreamBase(GameSession session)
        {
            _session = session;
            RC4Key = Convert.FromBase64String("F70IaxuU8C/w7FPXY1ibXw==");
        }
    }
}
