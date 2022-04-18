using System;
using System.Linq;
using Lexica.CLI.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lexica.CLI.Core.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddCoreModuleServices(this IServiceCollection services)
        {
            var executorsTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IService).IsAssignableFrom(x) && !x.IsInterface)
                .ToList<Type>();
            foreach (Type executorType in executorsTypes)
            {
                services.AddTransient(executorType);
            }
        }
    }
}