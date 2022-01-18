using LandmarkEmulator.Gateway.Network.Message;
using LandmarkEmulator.Shared;

namespace LandmarkEmulator.Gateway
{
    public class GatewayProvider : Singleton<GatewayProvider>
    {
        public void Initialise()
        {
            GatewayMessageManager.Instance.Initialise();
        }
    }
}
