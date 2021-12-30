using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.Ack, useEncryption: true)]
    public class Ack : IReadable, IWritable
    {
        public byte CompressionFlag { get; set; } = 0;
        public ushort Sequence { get; set; }

        public void Read(GamePacketReader reader)
        {
            CompressionFlag = reader.ReadByte();
            Sequence = reader.ReadUShortBE();
        }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(CompressionFlag);
            writer.Write(Sequence);
        }
    }
}
