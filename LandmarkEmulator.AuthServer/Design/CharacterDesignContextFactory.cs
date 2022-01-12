using LandmarkEmulator.Database.Character;
using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Shared.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace LandmarkEmulator.AuthServer.Design
{
    public class CharacterContextFactory : IDesignTimeDbContextFactory<CharacterContext>
    {
        public CharacterContext CreateDbContext(string[] args)
        {
            ConfigurationManager<AuthServerConfiguration>.Instance.Initialise("AuthServer.json");
            return new CharacterContext(new DatabaseConfig
            {
                Character = new DatabaseConnectionString
                {
                    Provider = DatabaseProvider.MySql,
                    ConnectionString = ConfigurationManager<AuthServerConfiguration>.Instance.Config.Database.Character.ConnectionString
                }
            });
        }
    }
}
