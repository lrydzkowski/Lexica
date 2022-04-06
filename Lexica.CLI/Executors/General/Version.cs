using Lexica.CLI.Core.Services;
using System;
using System.Collections.Generic;

namespace Lexica.CLI.Executors.General
{
    internal class Version : IExecutor
    {
        public Version(BuildService buildService, VersionService versionService)
        {
            BuildService = buildService;
            VersionService = versionService;
        }

        public BuildService BuildService { get; private set; }

        public VersionService VersionService { get; private set; }

        public void Execute(List<string>? args = null)
        {
            string version = VersionService.GetVersion();
            string build = BuildService.GetBuild();
            Console.Write($"Lexica {version} (CLI)");
            if (build.Length > 0)
            {
                Console.Write($" (build: {build})");
            }
            Console.WriteLine();
            Console.WriteLine("Copyright (c) Łukasz Rydzkowski");
        }
    }
}