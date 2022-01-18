using LandmarkEmulator.Gateway.Network;

namespace LandmarkEmulator.ZoneServer.Network
{
    public class ZoneSession : GatewaySession
    {
        public ZoneSession() : base(ZoneServer.EncryptionKey)
        {
        }
    }
}
