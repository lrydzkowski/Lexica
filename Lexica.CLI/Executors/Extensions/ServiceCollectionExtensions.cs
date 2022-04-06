using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Lexica.CLI.Executors.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddExecutorServices(this IServiceCollection services)
        {
            var executorsTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => (typeof(IExecutor).IsAssignableFrom(x) || typeof(IAsyncExecutor).IsAssignableFrom(x))
                            && !x.IsInterface)
                .ToList<Type>();
            foreach (Type executorType in executorsTypes)
            {
                services.AddTransient(executorType);
            }
        }
    }
}