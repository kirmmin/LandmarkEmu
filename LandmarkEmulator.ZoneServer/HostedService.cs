using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Gateway;
using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Configuration;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.GameTable;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.ZoneServer.Network;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading;
using System.Threading.Tasks;

namespace LandmarkEmulator.ZoneServer
{
    public class HostedService : IHostedService
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("Initialising...");

            DatabaseManager.Instance.Initialise(ConfigurationManager<ZoneServerConfiguration>.Instance.Config.Database);
            DatabaseManager.Instance.Migrate(DatabaseType.Auth);
            DatabaseManager.Instance.Migrate(DatabaseType.Character);

            GameTableManager.Instance.Initialise();

            //MessageManager.Instance.Initialise();
            GatewayProvider.Instance.Initialise();
            //TunnelDataManager.Instance.Initialise();

            //AuthAssetManager.Instance.Initialise();

            ThreadManager.Instance.Initialise(lastTick =>
            {
                NetworkManager<ZoneSession>.Instance.Update(lastTick);
            });

            NetworkManager<ZoneSession>.Instance.Initialise(ConfigurationManager<ZoneServerConfiguration>.Instance.Config.Network);

            //CommandManager.Instance.Initialise(new AuthCommandHandler());

            log.Info($"Service Started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info($"Ending Service...");

            NetworkManager<ZoneSession>.Instance.Shutdown();

            ThreadManager.Instance.Shutdown();

            log.Info($"Service Ended!");
            return Task.CompletedTask;
        }
    }
}
