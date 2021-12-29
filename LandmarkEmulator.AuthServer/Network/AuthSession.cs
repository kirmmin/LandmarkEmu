using LandmarkEmulator.Shared.Network;
using System.Net;
using System.Net.Sockets;

namespace LandmarkEmulator.AuthServer.Network
{
    public class AuthSession : GameSession
    {
        public override void OnAccept(IPEndPoint ep)
        {
            base.OnAccept(ep);

            log.Trace($"New session received on {ep.ToString()}");
        }
    }
}
