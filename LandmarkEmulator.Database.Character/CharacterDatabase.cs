using LandmarkEmulator.Database.Character.Model;
using LandmarkEmulator.Database.Configuration;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandmarkEmulator.Database.Character
{
    public class CharacterDatabase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IDatabaseConfig config;

        public CharacterDatabase(IDatabaseConfig config)
        {
            this.config = config;
        }

        public async Task Save(Action<CharacterContext> action)
        {
            await using var context = new CharacterContext(config);
            action.Invoke(context);
            await context.SaveChangesAsync();
        }

        public async Task Save(ISaveCharacter entity)
        {
            await using var context = new CharacterContext(config);
            entity.Save(context);
            await context.SaveChangesAsync();
        }

        public async Task Save(IEnumerable<ISaveCharacter> entities)
        {
            await using var context = new CharacterContext(config);
            foreach (ISaveCharacter entity in entities)
                entity.Save(context);
            await context.SaveChangesAsync();
        }

        public void Migrate()
        {
            using var context = new CharacterContext(config);

            List<string> migrations = context.Database.GetPendingMigrations().ToList();
            if (migrations.Count > 0)
            {
                log.Info($"Applying {migrations.Count} character database migration(s)...");
                foreach (string migration in migrations)
                    log.Info(migration);

                context.Database.Migrate();
            }
        }

        public List<CharacterModel> GetAllCharacters()
        {
            using var context = new CharacterContext(config);
            return context.Character.Where(c => c.DeleteTime == null).ToList();
        }

        public ulong GetNextCharacterId()
        {
            using var context = new CharacterContext(config);
            return context.Character
                .Select(r => r.Id)
                .DefaultIfEmpty()
                .Max();
        }

        public async Task<CharacterModel> GetCharacterById(ulong characterId)
        {
            await using var context = new CharacterContext(config);
            return await context.Character.FirstOrDefaultAsync(e => e.Id == characterId);
        }

        public async Task<CharacterModel> GetCharacterByName(string name)
        {
            await using var context = new CharacterContext(config);
            return await context.Character.FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<List<CharacterModel>> GetCharacters(uint accountId)
        {
            using var context = new CharacterContext(config);
            return await context.Character.Where(c => c.AccountId == accountId)
                .AsSplitQuery()
                .Include(c => c.Customisation)
                .ToListAsync();
        }

        public bool CharacterNameExists(string characterName)
        {
            using var context = new CharacterContext(config);
            return context.Character.Any(c => c.Name == characterName);
        }
    }
}
