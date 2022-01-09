using LandmarkEmulator.Database.Auth;
using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Shared.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace LandmarkEmulator.AuthServer.Design
{
    public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
    {
        public AuthContext CreateDbContext(string[] args)
        {
            ConfigurationManager<AuthServerConfiguration>.Instance.Initialise("AuthServer.json");
            return new AuthContext(new DatabaseConfig
            {
                Auth = new DatabaseConnectionString
                {
                    Provider = DatabaseProvider.MySql,
                    ConnectionString = ConfigurationManager<AuthServerConfiguration>.Instance.Config.Database.Auth.ConnectionString
                }
            });
        }
    }
}
