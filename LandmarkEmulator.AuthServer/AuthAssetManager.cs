using LandmarkEmulator.AuthServer.Network.Message.Model.TunnelData;
using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Database;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.IO;

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

        public ArtData ArtData { get; private set; }

        public void Initialise()
        {
            nextCharacterId = DatabaseManager.Instance.CharacterDatabase.GetNextCharacterId() + 1ul;
            InitiailiseArtData();
        }

        private void InitiailiseArtData()
        {
            using (StreamReader r = new StreamReader("Resources/ArtTints.json"))
            {
                string json = r.ReadToEnd();
                ArtData artData = JsonConvert.DeserializeObject<ArtData>(json);
                ArtData = artData;
            }
        }
    }
}
