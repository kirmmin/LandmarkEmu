using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network
{
    public class DataPacket
    {
        public byte[] Data { get; }
        public bool IsFragment { get; }

        public DataPacket(byte[] data, bool isFragment)
        {
            Data = data;
            IsFragment = isFragment;
        }
    }
}
