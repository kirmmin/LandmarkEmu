using System;

namespace LandmarkEmulator.Shared.Configuration
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message)
        {
        }
    }
}
