using Lexica.CLI.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Lexica.CLI.Core.Extensions
{
    static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
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
