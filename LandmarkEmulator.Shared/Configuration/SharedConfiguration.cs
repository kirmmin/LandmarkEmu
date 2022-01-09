﻿using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace LandmarkEmulator.Shared.Configuration
{
    public static class SharedConfiguration
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Initialise(string file)
        {
            if (Configuration != null)
                return;

            var builder = new ConfigurationBuilder()
                .AddJsonFile(file, false, true)
                .AddEnvironmentVariables()
                .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray());

            Configuration = builder.Build();
        }
    }
}
