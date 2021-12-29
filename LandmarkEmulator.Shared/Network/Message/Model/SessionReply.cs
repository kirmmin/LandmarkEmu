using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionReply)]
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
            writer.Write(SessionId);
            writer.Write(CRCSeed);
            writer.Write(CRCLength);
            writer.Write(Compression);
            writer.Write(UdpLength);
            writer.Write(Unknown0);
        }
    }
}
