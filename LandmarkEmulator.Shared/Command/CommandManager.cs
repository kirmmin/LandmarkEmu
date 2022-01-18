using NLog;
using System;
using System.Text;
using System.Threading;

namespace LandmarkEmulator.Shared.Command
{
    public class CommandManager : Singleton<CommandManager>
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private Thread commandThread;
        private readonly ManualResetEventSlim waitHandle = new();

        private volatile CancellationTokenSource cancellationToken;

        private ICommandHandler commandHandler;

        public void Initialise(ICommandHandler handler)
        {
            commandHandler = handler;

            InitialiseCommandThread();
        }

        private void InitialiseCommandThread()
        {
            cancellationToken = new CancellationTokenSource();

            commandThread = new Thread(CommandThread);
            commandThread.Start();

            // wait for command thread to start before continuing
            waitHandle.Wait();
        }

        private void CommandThread()
        {
            log.Info("Started command thread.");
            waitHandle.Set();

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write(">> ");
                var sb = Console.ReadLine();
                commandHandler?.Invoke(sb);
            }

            log.Info("Stopped command thread.");
        }
    }
}
