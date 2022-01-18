using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Gateway;
using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Configuration;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.GameTable;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using LandmarkEmulator.WorldServer.Network;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Threading;
using System.Threading.Tasks;

namespace LandmarkEmulator.WorldServer
{
    public class HostedService : IHostedService
    {
        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            log.Info("Initialising...");

            DatabaseManager.Instance.Initialise(ConfigurationManager<WorldServerConfiguration>.Instance.Config.Database);
            DatabaseManager.Instance.Migrate(DatabaseType.Auth);
            DatabaseManager.Instance.Migrate(DatabaseType.Character);

            GameTableManager.Instance.Initialise();

            MessageManager.Instance.Initialise();
            GatewayProvider.Instance.Initialise();
            //TunnelDataManager.Instance.Initialise();

            //AuthAssetManager.Instance.Initialise();

            ThreadManager.Instance.Initialise(lastTick =>
            {
                NetworkManager<WorldSession>.Instance.Update(lastTick);
            });

            NetworkManager<WorldSession>.Instance.Initialise(ConfigurationManager<WorldServerConfiguration>.Instance.Config.Network);

            //CommandManager.Instance.Initialise(new AuthCommandHandler());

            log.Info($"Service Started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            log.Info($"Ending Service...");

            NetworkManager<WorldSession>.Instance.Shutdown();

            ThreadManager.Instance.Shutdown();

            log.Info($"Service Ended!");
            return Task.CompletedTask;
        }
    }
}
