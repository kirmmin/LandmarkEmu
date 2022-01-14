using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Database;
using NLog;

namespace LandmarkEmulator.AuthServer
{
    public class AuthAssetManager : Singleton<AuthAssetManager>
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Id to be assigned to the next created character.
        /// </summary>
        public ulong NextCharacterId => nextCharacterId++;
        private ulong nextCharacterId;

        public void Initialise()
        {
            nextCharacterId = DatabaseManager.Instance.CharacterDatabase.GetNextCharacterId() + 1ul;
        }
    }
}
