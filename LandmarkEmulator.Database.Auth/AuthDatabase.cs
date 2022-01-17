using LandmarkEmulator.Database.Auth.Model;
using LandmarkEmulator.Database.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace LandmarkEmulator.Database.Auth
{
    public class AuthDatabase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IDatabaseConfig config;

        public AuthDatabase(IDatabaseConfig config)
        {
            this.config = config;
        }

        public async Task Save(Action<AuthContext> action)
        {
            using var context = new AuthContext(config);
            action.Invoke(context);
            await context.SaveChangesAsync();
        }

        public void Migrate()
        {
            using var context = new AuthContext(config);

            List<string> migrations = context.Database.GetPendingMigrations().ToList();
            if (migrations.Count > 0)
            {
                log.Info($"Applying {migrations.Count} authentication database migration(s)...");
                foreach (string migration in migrations)
                    log.Info(migration);

                context.Database.Migrate();
            }
        }

        /// <summary>
        /// Returns if an account with the given username already exists.
        /// </summary>
        public bool AccountExists(string username)
        {
            using var context = new AuthContext(config);
            return context.Account.SingleOrDefault(a => a.Username == username) != null;
        }

        /// <summary>
        /// Create a new account with the supplied email, salt and password verifier that is inserted into the database.
        /// </summary>
        public void CreateAccount(string username, string s, string v)
        {
            if (AccountExists(username))
                throw new InvalidOperationException($"Account with that username already exists.");

            using var context = new AuthContext(config);
            var model = new AccountModel
            {
                Username = username,
                Salt     = s,
                Verifier = v,
            };
            context.Account.Add(model);

            context.SaveChanges();
        }

        /// <summary>
        /// Delete an existing account with the supplied email.
        /// </summary>
        public bool DeleteAccount(string username)
        {
            using var context = new AuthContext(config);
            AccountModel account = context.Account.SingleOrDefault(a => a.Username == username);
            if (account == null)
                return false;

            context.Account.Remove(account);
            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// Update <see cref="AccountModel"/> with supplied session key asynchronously.
        /// </summary>
        public async Task UpdateAccountSessionKey(AccountModel account, string sessionKey)
        {
            account.SessionKey = sessionKey;
            account.SessionKeyExpiration = DateTime.Now.AddHours(0.5d); // Session Keys expire in 30 minutes

            await using var context = new AuthContext(config);
            EntityEntry<AccountModel> entity = context.Attach(account);
            entity.Property(p => p.SessionKey).IsModified = true;
            entity.Property(p => p.SessionKeyExpiration).IsModified = true;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Selects an <see cref="AccountModel"/> asynchronously that matches the supplied session key.
        /// </summary>
        public async Task<AccountModel> GetAccountBySessionKeyAsync(string sessionKey)
        {
            using var context = new AuthContext(config);
            return await context.Account
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.SessionKey == sessionKey && a.SessionKeyExpiration > DateTime.Now);
        }

        /// <summary>
        /// Selects an <see cref="AccountModel"/> asynchronously that matches the supplied session key.
        /// </summary>
        public async Task<AccountModel> GetAccountByUsername(string username)
        {
            using var context = new AuthContext(config);
            return await context.Account
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.Username == username);
        }

        /// <summary>
        /// Update <see cref="AccountModel"/> with supplied server ticket asynchronously.
        /// </summary>
        public async Task UpdateServerTicket(AccountModel account, string serverTicket)
        {
            account.ServerTicket = serverTicket;

            await using var context = new AuthContext(config);
            EntityEntry<AccountModel> entity = context.Attach(account);
            entity.Property(p => p.ServerTicket).IsModified = true;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Selects an <see cref="AccountModel"/> asynchronously that matches the supplied server ticket.
        /// </summary>
        public async Task<AccountModel> GetAccountByServerTicketAsync(string serverTicket)
        {
            using var context = new AuthContext(config);
            return await context.Account
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.ServerTicket == serverTicket);
        }

        /// <summary>
        /// Selects all Zone Servers and returns a list containing all <see cref="ZoneServerModel"/>.
        /// </summary>
        public ImmutableList<ZoneServerModel> GetZoneServers()
        {
            using var context = new AuthContext(config);
            return context.ZoneServer
                .AsNoTracking()
                .ToImmutableList();
        }
    }
}
