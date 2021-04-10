using Lexica.CLI.Core.Services;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lexica.CLI.Executors.General
{
    class Version : IExecutor
    {
        private readonly BuildService _buildService;

        public Version(BuildService buildService)
        {
            _buildService = buildService;
        }

        public void Execute(List<string>? args = null)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
            string build = _buildService.GetBuild();
            if (build.Length == 0)
            {
                build = "-";
            }
            Console.WriteLine($"Lexica {version} (CLI) (build: {build})");
            Console.WriteLine("Copyright (c) Łukasz Rydzkowski");
        }
    }
}
