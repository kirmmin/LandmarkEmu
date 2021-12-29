using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Network
{
    public class InvalidPacketValueException : Exception
    {
        public InvalidPacketValueException()
        {
        }

        public InvalidPacketValueException(string message)
            : base(message)
        {
        }
    }
}
