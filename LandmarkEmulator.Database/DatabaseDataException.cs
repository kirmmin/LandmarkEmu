using System;

namespace LandmarkEmulator.Database
{
    public class DatabaseDataException : Exception
    {
        public DatabaseDataException(string message)
            : base(message)
        {
        }
    }
}
