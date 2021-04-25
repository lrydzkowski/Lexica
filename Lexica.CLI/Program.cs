using Lexica.CLI.Args;
using Lexica.CLI.Core;
using Lexica.CLI.Core.Config;
using Lexica.CLI.Core.Extensions;
using Lexica.CLI.Executors.Extensions;
using Lexica.Core.Extensions;
using Lexica.Core.Services;
using Lexica.Database;
using Lexica.Database.Extensions;
using Lexica.Pronunciation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Lexica.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                if (args.Length == 0)
                {
                    args = new string[] { "--help" };
                }
                var servicesProvider = ConfigureServices();
                using (servicesProvider as IDisposable)
                {
                    try
                    {
                        await ArgsHandler.RunAsync(args, servicesProvider);
                    }
                    catch (ArgsException ex)
                    {
                        View.ShowError(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
                View.ShowError(ex);
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault 
                // on Linux)
                LogManager.Shutdown();
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            AddNLogService(services);
            ConfigService<AppSettings> configService = AddConfigService(services);
            AddPronunciationService(services, configService);
            services.AddExecutorServices();
            services.AddCoreModuleServices();
            services.AddDatabaseServices();
            services.AddCoreLibraryServices();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        private static void AddNLogService(ServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
            });
        }

        private static ConfigService<AppSettings> AddConfigService(ServiceCollection services)
        {
            ConfigService<AppSettings> configService = ConfigService<AppSettings>.Get(
                "appsettings", Assembly.GetExecutingAssembly()
            );
            services.AddSingleton(configService);
            return configService;
        }

        private static void AddPronunciationService(
            ServiceCollection services, 
            ConfigService<AppSettings> configService)
        {
            services.AddSingleton(
                configService.Get().PronunciationApi?.WebDictionary
                ?? new Pronunciation.Api.WebDictionary.Config.WebDictionarySettings()
            );
            services.AddSingleton<IPronunciation, Pronunciation.Api.WebDictionary.PronunciationService>();
        }
    }
}
