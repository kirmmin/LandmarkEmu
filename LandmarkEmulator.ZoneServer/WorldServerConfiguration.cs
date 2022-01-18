using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Shared.Configuration;

namespace LandmarkEmulator.WorldServer
{
    public class WorldServerConfiguration
    {
        public NetworkConfig Network { get; set; }
        public DatabaseConfig Database { get; set; }
        public string GameDataPath { get; set; }
    }
}
