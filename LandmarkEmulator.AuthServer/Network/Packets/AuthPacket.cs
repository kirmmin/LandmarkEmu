﻿using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using System.Collections.Generic;

namespace LandmarkEmulator.AuthServer.Network.Packets
{
    public class AuthPacket
    {
        /// <summary>
        /// Total size including the header and payload.
        /// </summary>
        public uint Size { get; protected set; }
        public AuthMessageOpcode Opcode { get; protected set; }

        public byte[] Data { get; protected set; }

        public AuthPacket(byte[] data)
        {
            var reader = new GamePacketReader(data);

            Opcode = (AuthMessageOpcode)reader.ReadByte();
            Data = reader.ReadBytes(reader.BytesRemaining);
            Size = (uint)Data.Length;
        }

        public AuthPacket(AuthMessageOpcode opcode, IWritable message)
        {
            Opcode = opcode;

            List<byte> data = new();
            var writer = new GamePacketWriter(data);
            message.Write(writer);

            Data = data.ToArray();
            Size = (uint)Data.Length;
        }
    }
}
