using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionReply, useEncryption: true)]
    public class SessionReply : IWritable
    {
        public uint SessionId { get; set; }
        public uint CRCSeed { get; set; }
        public byte CRCLength { get; set; }
        public ushort Compression { get; set; }
        public uint UdpLength { get; set; }
        public uint Unknown0 { get; set; } = 3;

        public void Write(GamePacketWriter writer)
        {
            writer.WriteBE(SessionId);
            writer.WriteBE(CRCSeed);
            writer.Write(CRCLength);
            writer.WriteBE(Compression);
            writer.WriteBE(UdpLength);
            writer.WriteBE(Unknown0);
        }
    }
}
