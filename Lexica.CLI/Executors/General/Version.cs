using Lexica.CLI.Core.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lexica.CLI.Executors.General
{
    class Version : IExecutor
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
            if (build.Length == 0)
            {
                build = "-";
            }
            Console.WriteLine($"Lexica {version} (CLI) (build: {build})");
            Console.WriteLine("Copyright (c) Łukasz Rydzkowski");
        }
    }
}
