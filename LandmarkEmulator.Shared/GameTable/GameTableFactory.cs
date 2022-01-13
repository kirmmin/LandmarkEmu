using LandmarkEmulator.Shared.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace LandmarkEmulator.Shared.GameTable
{
    public static class GameTableFactory
    {
        public static object LoadGameTable(Type type, string fileName, bool hasId)
        {
            string path = SharedConfiguration.Configuration.GetValue("GameDataPath", "gameData");
            string filePath = Path.Combine(path, fileName);

            Type table = typeof(GameTable<>).MakeGenericType(type);
            return Activator.CreateInstance(table, filePath, hasId);
        }
    }
}
