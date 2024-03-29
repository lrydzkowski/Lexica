﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Lexica.CLI.Args.Models;
using Lexica.CLI.Executors;
using Lexica.Core.IO;
using Lexica.Core.Services;

namespace Lexica.CLI.Args
{
    internal static class ArgsHandler
    {
        public static async Task RunAsync(string[] args, IServiceProvider servicesProvider)
        {
            var cliMapSource = new EmbeddedSource("cli-map.json", Assembly.GetExecutingAssembly());
            var cliMapSchemaSource = new EmbeddedSource("cli-map.schema.json", Assembly.GetExecutingAssembly());
            var cliMapService = new ConfigService<CLIMap>(cliMapSource, cliMapSchemaSource);
            List<Command>? commands = cliMapService.Config?.Commands;
            if (commands == null)
            {
                throw new ArgsException("CLI map has empty commands array.");
            }

            var commandsToExecute = new List<Command>();
            foreach (string arg in args)
            {
                Command? command = commands.Find(x => x.Name == arg || x.Shortcut == arg);
                if (commandsToExecute.Count == 0 && command == null)
                {
                    throw new ArgsException($"Argument {arg} isn't handled.");
                }
                if (command == null)
                {
                    commandsToExecute[^1].Parameters.Add(arg);
                }
                else
                {
                    commandsToExecute.Add(command);
                    commands = command.Commands;
                }
            }

            for (int i = 0; i < commandsToExecute.Count; i++)
            {
                Command command = commandsToExecute[i];
                string? executorClassName = null;
                if (command.ExecutorClass != null)
                {
                    executorClassName = command.ExecutorClass;
                }
                else if (i == commandsToExecute.Count - 1 && command.DefaultExecutorClass != null)
                {
                    executorClassName = command.DefaultExecutorClass;
                }
                if (executorClassName != null)
                {
                    Type? type = Type.GetType(executorClassName);
                    if (type == null)
                    {
                        throw new ArgsException($"Type {executorClassName} doesn't exist.");
                    }
                    List<string> parameters = command.Parameters;
                    if (command.AddNameToArguments == true)
                    {
                        parameters.Insert(0, command.Name);
                    }

                    if (typeof(IExecutor).IsAssignableFrom(type))
                    {
                        IExecutor? syncExecutor = (IExecutor?)servicesProvider.GetService(type);
                        if (syncExecutor == null)
                        {
                            throw new Exception($"Service {executorClassName} doesn't exist.");
                        }
                        syncExecutor.Execute(parameters);
                        return;
                    }
                    else if (typeof(IAsyncExecutor).IsAssignableFrom(type))
                    {
                        IAsyncExecutor? asyncExecutor = (IAsyncExecutor?)servicesProvider.GetService(type);
                        if (asyncExecutor == null)
                        {
                            throw new Exception($"Service {executorClassName} doesn't exist.");
                        }
                        await asyncExecutor.ExecuteAsync(parameters);
                        return;
                    }
                    else
                    {
                        throw new ArgsException($"Type {executorClassName} doesn't implement IExecutor or IAsynExecutor.");
                    }
                }
            }
        }
    }
}