using LandmarkEmulator.Database.Auth;
using LandmarkEmulator.Database.Configuration;
using NLog;
using System;

namespace LandmarkEmulator.Shared.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public AuthDatabase AuthDatabase { get; private set; }
        //public CharacterDatabase CharacterDatabase { get; private set; }
        //public WorldDatabase WorldDatabase { get; private set; }

        private DatabaseManager()
        {
        }

        public void Initialise(DatabaseConfig config)
        {
            if (config.Auth != null)
                AuthDatabase = new AuthDatabase(config);

            //if (config.Character != null)
            //    CharacterDatabase = new CharacterDatabase(config);

            //if (config.World != null)
            //    WorldDatabase = new WorldDatabase(config);
        }

        public void Migrate(DatabaseType type)
        {
            log.Info("Applying database migrations...");

            try
            {
                switch (type)
                {
                    case DatabaseType.Auth:
                        AuthDatabase.Migrate();
                        break;
                }
                //CharacterDatabase.Migrate();
                //WorldDatabase.Migrate();
            }
            catch (Exception exception)
            {
                log.Fatal(exception);
                throw;
            }
        }
    }
}
