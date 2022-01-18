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

        /// <summary>
        /// Used to determine whether the Arc4 Encryption is active.
        /// </summary>
        public bool UsingEncryption { get; private set; } = true;

        public byte[] RC4Key { get; private set; }

        protected readonly DataPacket[] DataPackets = new DataPacket[ushort.MaxValue];

        public DataStreamBase(GameSession session)
        {
            _session = session;
            RC4Key = Convert.FromBase64String(session.EncryptionKey);
        }

        /// <summary>
        /// Sets whether this <see cref="DataStreamBase"/> is currently using the Arc4 Encryption on the packets it processes.
        /// </summary>
        public void SetEncryption(bool usingEncryption)
        {
            UsingEncryption = usingEncryption;
        }
    }
}
