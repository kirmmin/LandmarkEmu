using LandmarkEmulator.Shared.GameTable.Model;
using LandmarkEmulator.Shared.GameTable.Text;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LandmarkEmulator.Shared.GameTable
{
    public sealed class GameTableManager : Singleton<GameTableManager>
    {
        private const int minimumThreads = 2;
        private const int maximumThreads = 16;

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        [GameData("BlackListEntries.txt", false)]
        public GameTable<BlackListEntry> BlackList { get; private set; }

        /// <summary>
        /// Initialises all <see cref="GameTable{T}"/> that the <see cref="GameTableManager"/> caches. This initialises the <see cref="TextManager"/>, first.
        /// </summary>
        public void Initialise()
        {
            TextManager.Instance.Initialise();
            
            log.Info("Loading GameTables...");

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                LoadGameTablesAsync().GetAwaiter().GetResult();
                //Debug.Assert(WorldLocation2 != null);
            }
            catch (Exception exception)
            {
                log.Fatal(exception);
                throw;
            }

            log.Info($"Loaded GameTables in {sw.ElapsedMilliseconds}ms.");
        }

        private async Task LoadGameTablesAsync()
        {
            var exceptions = new List<Exception>();
            int loadCount = Environment.ProcessorCount * 2;
            if (loadCount < minimumThreads)
                loadCount = minimumThreads;
            if (loadCount > maximumThreads)
                loadCount = maximumThreads;

            var tasks = new List<Task>();
            async Task WaitForNextTaskToFinish()
            {
                Task next = await Task.WhenAny(tasks);
                tasks.Remove(next);
            }

            async Task<bool> ExceptionHandler(Task task)
            {
                try
                {
                    await task;
                    return true;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    return false;
                }
            }

            string GetFilename(PropertyInfo property)
            {
                GameDataAttribute attribute = property.GetCustomAttribute<GameDataAttribute>();
                if (attribute == null)
                    return null;

                string fileName = attribute.FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = property.Name;

                    if (property.PropertyType.GetGenericTypeDefinition() == typeof(GameTable<>))
                        fileName = Path.ChangeExtension(fileName, "txt");
                }

                return fileName;
            }

            bool GetHasId(PropertyInfo property)
            {
                GameDataAttribute attribute = property.GetCustomAttribute<GameDataAttribute>();
                if (attribute == null)
                    return true;

                return attribute.HasId;
            }

            var properties = new List<PropertyInfo>();
            foreach (PropertyInfo property in typeof(GameTableManager).GetProperties())
            {
                GameDataAttribute attribute = property.GetCustomAttribute<GameDataAttribute>();
                if (attribute == null)
                    continue;
                
                properties.Add(property);
            }

            foreach (PropertyInfo property in properties)
            {
                string fileName = GetFilename(property);
                bool hasId = GetHasId(property);

                DateTime loadStarted = DateTime.Now;

                tasks.Add(LoadGameTableAsync(property, fileName, hasId)
                    .ContinueWith(ExceptionHandler)
                    .Unwrap()
                    .ContinueWith(
                        async task =>
                        {
                            bool result = await task;
                            if (result)
                                log.Info("Completed loading {0} in {1}ms", fileName,
                                    (DateTime.Now - loadStarted).TotalMilliseconds);
                            else
                                log.Error("Failed to load {0} in {1}ms", fileName,
                                    (DateTime.Now - loadStarted).TotalMilliseconds);
                        }).Unwrap());

                if (tasks.Count > loadCount)
                    await WaitForNextTaskToFinish();
            }

            while (tasks.Count > 0)
                await WaitForNextTaskToFinish();

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

        private Task LoadGameTableAsync(PropertyInfo property, string fileName, bool hasId)
        {
            async Task SetPropertyOnCompletion(Task<object> task)
            {
                property.SetValue(this, await task);
            }

            async Task VerifyPropertySetOnCompletion(Task task)
            {
                await task;
                if (property.GetValue(this) == null)
                    throw new InvalidOperationException($"Failed to load game data table {Path.GetFileName(fileName)}");
            }

            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(GameTable<>))
                return Task.Factory.StartNew(() =>
                        GameTableFactory.LoadGameTable(property.PropertyType.GetGenericArguments().Single(), fileName, hasId))
                            .ContinueWith(SetPropertyOnCompletion)
                            .Unwrap()
                            .ContinueWith(VerifyPropertySetOnCompletion)
                            .Unwrap();

            throw new GameTableException($"Unknown game table type {property.PropertyType}");
        }
    }
}
