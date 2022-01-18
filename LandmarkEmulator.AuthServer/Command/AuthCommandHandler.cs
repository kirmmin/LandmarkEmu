using LandmarkEmulator.Shared.Command;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.Network.Cryptography;
using NLog;
using System;

namespace LandmarkEmulator.AuthServer.Command
{
    public class AuthCommandHandler : ICommandHandler
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        // TODO: Burn this to the ground and refactor in to a real system :)
        public void Invoke(string command)
        {
            string[] parameters = command.Split(" ");

            if (parameters.Length == 4)
            {
                if (parameters[0] == "account")
                {
                    if (parameters[1] == "create")
                    {
                        Write($"{HandleAccountCreate(parameters[2], parameters[3])}");
                    }
                    else
                        Write($"{{=Red}}Command formatting incorrect. Must be \"account create username password\".");
                }
                else
                    Write($"{{=Red}}Command {parameters[0]} not recognised.");
            }
            else
                Write($"{{=Red}}Command {parameters[0]} not recognised.");
        }

        private string HandleAccountCreate(string username, string password)
        {
            if (DatabaseManager.Instance.AuthDatabase.AccountExists(username))
                return $"{{=Orange}}Username already exists. Please try another.";

            var (s, v) = PasswordProvider.GenerateSaltAndVerifier(password);
            DatabaseManager.Instance.AuthDatabase.CreateAccount(username, s, v);
            return $"{{=Green}}Account {username} successfully created!";
        }

        public void Write(string msg)
        {
            string[] ss = msg.Split('{', '}');
            ConsoleColor c;
            foreach (var s in ss)
                if (s.StartsWith("/"))
                    Console.ResetColor();
                else if (s.StartsWith("=") && Enum.TryParse(s.Substring(1), out c))
                    Console.ForegroundColor = c;
                else
                {
                    Console.Write(s + "\n");
                    Console.ResetColor();
                }
        }
    }
}
