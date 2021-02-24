using Lexica.CLI.Args.Models;
using Lexica.CLI.Core.Services;
using Lexica.CLI.Executors;
using Lexica.Core.IO;
using Lexica.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lexica.CLI.Args
{
    class ArgsService : IService
    {
        public async Task RunAsync(string[] args, IServiceProvider servicesProvider)
        {
            var cliMapSource = new EmbeddedSource("cli-map.json", Assembly.GetExecutingAssembly());
            var cliMapSchemaSource = new EmbeddedSource("cli-map.schema.json", Assembly.GetExecutingAssembly());
            var cliMapService = new ConfigService<CLIMap>(cliMapSource, cliMapSchemaSource);
            List<Command>? commands = cliMapService.Config?.Commands;
            if (commands == null)
            {
                throw new ArgsException("CLI map has empty commands array.");
            }

            Type syncExecutorType = typeof(IExecutor);
            Type asyncExecutorType = typeof(IAsyncExecutor);

            foreach (string arg in args)
            {
                Command? command = commands.Where(x => x.Name == arg || x.Shortcut == arg).FirstOrDefault();
                if (command == null)
                {
                    throw new ArgsException($"Argument {arg} isn't handled.");
                }
                if (command.ExecutorClass != null)
                {
                    Type? type = Type.GetType(command.ExecutorClass);
                    if (type == null)
                    {
                        throw new ArgsException($"Type {command.ExecutorClass} from command {arg} doesn't exist.");
                    }
                    
                    if (syncExecutorType.IsAssignableFrom(type))
                    {
                        IExecutor syncExecutor = (IExecutor)servicesProvider.GetService(type);
                        syncExecutor.Execute();
                    } 
                    else if (asyncExecutorType.IsAssignableFrom(type))
                    {
                        IAsyncExecutor asyncExecutor = (IAsyncExecutor)servicesProvider.GetService(type);
                        await asyncExecutor.ExecuteAsync();
                    }
                    else
                    {
                        throw new ArgsException($"Type {command.ExecutorClass} from command {arg} doesn't implement IExecutor or IAsynExecutor.");
                    }
                }
            }
        }
    }
}
