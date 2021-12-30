using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network.Message.Model
{
    [ProtocolMessage(ProtocolMessageOpcode.SessionRequest, useEncryption: false)]
    public class SessionRequest : IReadable
    {
        public uint CRCLength { get; set; }
        public uint SessionId { get; set; }
        public uint UdpLength { get; set; }
        public string Protocol { get; set; }

        public void Read(GamePacketReader reader)
        {
            CRCLength = reader.ReadUIntBE();
            SessionId = reader.ReadUIntBE();
            UdpLength = reader.ReadUIntBE();
            Protocol  = reader.ReadNullTerminatedString();
        }
    }
}
