using System;

namespace LandmarkEmulator.Shared.GameTable
{
    public class GameTableException : Exception
    {
        public GameTableException(string message)
            : base(message)
        {
        }
    }
}
