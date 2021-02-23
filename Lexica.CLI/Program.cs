using Lexica.CLI.Config;
using Lexica.Core.IO;
using Lexica.Core.Services;
using Lexica.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Reflection;

namespace Lexica.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var servicesProvider = ConfigureServices();
                using (servicesProvider as IDisposable)
                {
                    Console.WriteLine("Ok!");
                    Console.WriteLine("Press ANY key to exit");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault 
                // on Linux)
                LogManager.Shutdown();
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // NLog
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            // Configuration
            ConfigService<AppSettings> configService = ConfigService<AppSettings>.Get(
                "appsettings", Assembly.GetExecutingAssembly()
            );
            services.AddSingleton<ConfigService<AppSettings>>(configService);

            // Entity Framework Core
            services.AddDbContext<LexicaContext>(
                opts => opts.UseNpgsql(configService.Get().Database?.ConnectionString, x => x.MigrationsAssembly("Lexica.EF"))
            );

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
