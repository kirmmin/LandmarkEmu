﻿namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Ack, useEncryption: true)]
    public class Ack : IProtocol
    {
        public byte CompressionFlag { get; set; } = 0;
        public ushort Sequence { get; set; }

        public void Read(GamePacketReader reader, PacketOptions options)
        {
            if (!options.IsSubpacket)
                CompressionFlag = reader.ReadByte();
            Sequence = reader.ReadUShortBE();
        }

        public void Write(GamePacketWriter writer, PacketOptions options)
        {
            if (!options.IsSubpacket)
                writer.Write(CompressionFlag);
            writer.WriteBE(Sequence);
        }
    }
}
