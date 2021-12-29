using LandmarkEmulator.AuthServer.Network;
using LandmarkEmulator.Shared;
using LandmarkEmulator.Shared.Network;
using LandmarkEmulator.Shared.Network.Message;
using NLog;
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

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.Title = Title;
            log.Info("Initialising...");

            MessageManager.Initialise();
            NetworkManager<AuthSession>.Initialise("0.0.0.0", 20042);

            ServerManager.Initialise(lastTick =>
            {
                NetworkManager<AuthSession>.Update(lastTick);
            });
        }
    }
}
