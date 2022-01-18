using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.Command
{
    public interface ICommandHandler
    {
        void Invoke(string command);
    }
}
