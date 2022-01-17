using LandmarkEmulator.Shared.Configuration;
using LandmarkEmulator.WebAPI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace LandmarkEmulator.AuthServer
{
    public static class AuthServer
    {
        #if DEBUG
        private const string Title = "LandmarkEmulator: Auth Server (DEBUG)";
        #else
        private const string Title = "LandmarkEmulator: World Server (RELEASE)";
        #endif

        private static readonly NLog.ILogger log = LogManager.GetCurrentClassLogger();

        public const string EncryptionKey = "F70IaxuU8C/w7FPXY1ibXw==";

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            ConfigurationManager<AuthServerConfiguration>.Instance.Initialise("AuthServer.json");

            IHostBuilder builder = new HostBuilder()
                .ConfigureLogging(lb =>
                {
                    // only applicable to logging done through host
                    // other logging is still done directly though NLog
                    lb.ClearProviders()
                        .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace)
                        .AddNLog();
                })
                .ConfigureServices(sc =>
                {
                    sc.AddHostedService<HostedService>();
                    sc.AddHostedService(service => 
                        new AuthWebAPI(
                            ConfigurationManager<AuthServerConfiguration>.Instance.Config.WebApi, 
                            ConfigurationManager<AuthServerConfiguration>.Instance.Config.Database)
                        );
                })
                .UseWindowsService()
                .UseSystemd();

            if (!WindowsServiceHelpers.IsWindowsService() && !SystemdHelpers.IsSystemdService())
                Console.Title = Title;

            try
            {
                IHost host = builder.Build();
                host.Run();
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }
    }
}
