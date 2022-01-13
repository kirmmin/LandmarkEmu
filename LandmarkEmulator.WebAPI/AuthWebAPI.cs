using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Database.Configuration;
using LandmarkEmulator.Shared.Database;
using LandmarkEmulator.Shared.Game;
using LandmarkEmulator.Shared.Network.Cryptography;
using LandmarkEmulator.WebAPI.Configuration;
using LandmarkEmulator.WebAPI.Models.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LandmarkEmulator.WebAPI
{
    public class AuthWebAPI : IHostedService
    {
        bool enabled;
        WebApplication? app;
        string hostname;

        public AuthWebAPI(WebApiConfig config, DatabaseConfig databaseConfig)
        {
            enabled = config.Enabled;
            if (!enabled)
                return;

            hostname = config.BaseURI ?? "https://0.0.0.0:5000";
            DatabaseManager.Instance.Initialise(databaseConfig);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!enabled)
                return Task.CompletedTask;

            var builder = WebApplication.CreateBuilder();
            app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            // Creates an account and returns a CreateModel containing the Username if successful.
            app.MapPost("/create", async (string username, string password) =>
            {
                if (DatabaseManager.Instance.AuthDatabase.AccountExists(username))
                    return Results.Conflict(new CreateModel
                    {
                        Message  = "Unable to create account. Username already exists."
                    });

                var (s, v) = PasswordProvider.GenerateSaltAndVerifier(password);
                DatabaseManager.Instance.AuthDatabase.CreateAccount(username, s, v);

                return Results.Ok(new CreateModel
                {
                    Username = username,
                    Message = "Success."
                });
            });
            
            // Returns am AuthenticateModel containing a SessionKey upon successful authentication.
            app.MapPost("/authenticate", async (string username, string password) =>
            {
                AccountModel account = await DatabaseManager.Instance.AuthDatabase.GetAccountByUsername(username);
                if (account == null)
                    return Results.StatusCode(403);

                if (!PasswordProvider.VerifyPassword(account.Salt, account.Verifier, password))
                    return Results.StatusCode(403);

                string sessionKey = RandomProvider.GetBytes(16u).ToHexString();
                await DatabaseManager.Instance.AuthDatabase.UpdateAccountSessionKey(account, sessionKey);

                return Results.Ok(new AuthenticateModel
                {
                    SessionKey = sessionKey,
                    SessionKeyExpiration = DateTime.Now.AddMinutes(30d),
                    Message = "Success."
                });
            });

            app.Urls.Clear();
            app.Urls.Add(hostname);

            app.StartAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            app?.StopAsync(cancellationToken);
            
            return Task.CompletedTask;
        }
    }
}
