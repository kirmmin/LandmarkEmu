using LandmarkEmulator.AuthServer.Network;
using LandmarkEmulator.AuthServer.Network.Message;
using LandmarkEmulator.AuthServer.Zone;
using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Configuration;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.Game.Text;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading;
using System.Threading.Tasks;

namespace LandmarkEmulator.AuthServer
{
    public class HostedService : IHostedService
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("Initialising...");

            ConfigurationManager<AuthServerConfiguration>.Instance.Initialise("AuthServer.json");

            DatabaseManager.Instance.Initialise(ConfigurationManager<AuthServerConfiguration>.Instance.Config.Database);
            DatabaseManager.Instance.Migrate(DatabaseType.Auth);

            TextManager.Instance.Initialise();

            MessageManager.Instance.Initialise();
            AuthMessageManager.Instance.Initialise();
            TunnelDataManager.Instance.Initialise();

            ZoneServerManager.Instance.Initialise();
            NetworkManager<AuthSession>.Instance.Initialise(ConfigurationManager<AuthServerConfiguration>.Instance.Config.Network);

            ThreadManager.Instance.Initialise(lastTick =>
            {
                NetworkManager<AuthSession>.Instance.Update(lastTick);
            });

            log.Info($"Service Started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info($"Ending Service...");

            NetworkManager<AuthSession>.Instance.Shutdown();

            ThreadManager.Instance.Shutdown();

            log.Info($"Service Ended!");
            return Task.CompletedTask;
        }
    }
}
