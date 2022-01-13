using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace LandmarkEmulator.Shared.GameTable.Text
{
    public class TextManager : Singleton<TextManager>
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private Dictionary<uint, string> hashToString = new();
        private Dictionary<uint, uint> keyToHash = new();

        public void Initialise()
        {
            log.Info("Loading TextTables...");
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                LoadDataFile("en_us_data.dat");
            }
            catch (Exception exception)
            {
                log.Fatal(exception);
                throw;
            }
            log.Info($"Loaded TextTables with {hashToString.Count} entries in {sw.ElapsedMilliseconds}ms.");

            log.Info($"Building ReverseLookup...");
            sw.Restart();
            uint key = 0;
            while (key < 1000000u)
            {
                uint hash = GetHash(key);
                if (hashToString.TryGetValue(hash, out string hashVal))
                    keyToHash.Add(key, hash);

                key++;
            }
            log.Info($"Built {keyToHash.Count} reverse lookups in {sw.ElapsedMilliseconds}ms.");
        }

        private void LoadDataFile(string filePath)
        {
            using (StreamReader reader = File.OpenText(filePath))
            {
                string line = String.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    // TODO: Handle string entries that have multiple lines.
                    if (!Regex.IsMatch(line, @"^\d+"))
                        continue;

                    var pieces = Regex.Split(line, @"\t");
                    hashToString.TryAdd(Convert.ToUInt32(pieces[0]), pieces[2]);
                }
            }
        }

        /// <summary>
        /// Get Text Hash for a given NAME_ID.
        /// </summary>
        public uint GetHash(uint id)
        {
            return GetHash($"Global.Text.{id}");
        }

        /// <summary>
        /// Get Text Hash for a given global string identifier.
        /// </summary>
        public uint GetHash(string id)
        {
            if (!id.StartsWith("Global.Text"))
                id = "Global.Text." + id;

            return JenkinsLookup2.Lookup(id);
        }

        /// <summary>
        /// Get a Key that matches a given Text Hash.
        /// </summary>
        public uint? GetIdForHash(uint hash)
        {
            if (hashToString.ContainsKey(hash))
            {
                if (keyToHash.ContainsValue(hash))
                {
                    foreach (var (keyVal, val) in keyToHash)
                    {
                        if (val == hash)
                            return keyVal;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the string that would match the given NAME_ID.
        /// </summary>
        public string GetTextForId(uint id)
        {
            if (keyToHash.TryGetValue(id, out uint hash))
                return hashToString.TryGetValue(hash, out string text) ? text : "";

            return "";
        }
    }
}
