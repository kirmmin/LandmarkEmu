using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Shared.Configuration;
using LandmarkEmulator.WebAPI.Configuration;

namespace LandmarkEmulator.AuthServer
{
    public class AuthServerConfiguration
    {
        public NetworkConfig Network { get; set; }
        public DatabaseConfig Database { get; set; }
        public WebApiConfig WebApi { get; set; }
    }
}
