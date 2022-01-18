using LandmarkEmulator.Database.Character.Model;
using LandmarkEmulator.Gateway.Network;

namespace LandmarkEmulator.WorldServer.Network
{
    public class WorldSession : GatewaySession
    {
        public WorldSession() : base(WorldServer.EncryptionKey)
        {
        }

        public override void OnCharacterLogin(CharacterModel model)
        {
            base.OnCharacterLogin(model);

            // Setup Player, Send Packets, etc.
        }
    }
}
