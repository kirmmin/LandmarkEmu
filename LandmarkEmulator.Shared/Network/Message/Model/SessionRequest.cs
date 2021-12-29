using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionRequest)]
    public class SessionRequest : IReadable
    {
        public uint CRCLength { get; set; }
        public uint SessionId { get; set; }
        public uint UdpLength { get; set; }
        public string Protocol { get; set; }

        public void Read(GamePacketReader reader)
        {
            CRCLength = reader.ReadUInt();
            SessionId = reader.ReadUInt();
            UdpLength = reader.ReadUInt();
            Protocol  = reader.ReadNullTerminatedString();
        }
    }
}
