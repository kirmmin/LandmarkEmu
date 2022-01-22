using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Parser
{
    public class UdpHeader
    {
        public ushort SourcePort { get; private set; }
        public ushort DestinationPort { get; private set; }

        public UdpHeader(byte[] byBuffer, int nReceived)
        {
            // Create MemoryStream out of the received bytes. 
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            // Next we create a BinaryReader out of the MemoryStream. 
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            // First 2 bytes are source port
            var srcPort = binaryReader.ReadBytes(2);
            Array.Reverse(srcPort);
            SourcePort = BitConverter.ToUInt16(srcPort, 0);
            // The next 2 bytes are destination port
            var dstPort = binaryReader.ReadBytes(2);
            Array.Reverse(dstPort);
            DestinationPort = BitConverter.ToUInt16(dstPort, 0);
        }
    }
}
