﻿using LandmarkEmulator.Database.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace LandmarkEmulator.Database
{
    public static class Extensions
    {
        public static DbContextOptionsBuilder UseConfiguration(this DbContextOptionsBuilder optionsBuilder, IDatabaseConfig databaseConfiguration, DatabaseType databaseType)
        {
            var connectionString = databaseConfiguration.GetConnectionString(databaseType);
            switch (connectionString.Provider)
            {
                case DatabaseProvider.MySql:
                    optionsBuilder.UseMySql(connectionString.ConnectionString, ServerVersion.AutoDetect(connectionString.ConnectionString), b =>
                    {
                        b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });
                    break;
                default:
                    throw new NotSupportedException($"The requested database provider: {connectionString.Provider:G} is not supported.");
            }
            return optionsBuilder;
        }
    }
}
