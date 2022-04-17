using System;
using System.Threading.Tasks;
using Lexica.CLI.Args;
using Lexica.CLI.Core;
using Lexica.CLI.Core.Extensions;
using Lexica.CLI.Executors.Extensions;
using Lexica.Core.Extensions;
using Lexica.Database.Extensions;
using Lexica.Learning.Config;
using Lexica.Pronunciation;
using Lexica.Pronunciation.Config;
using Lexica.Words.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;

namespace Lexica.CLI
{
    internal class Program
    {
        private static async Task Main(string[] args)
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
            AddConfigServices(services);
            AddPronunciationService(services);
            services.AddExecutorServices();
            services.AddCoreModuleServices();
            services.AddDatabaseServices();
            services.AddCoreLibraryServices();
            return services.BuildServiceProvider();
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

        private static void AddConfigServices(ServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();
            AddConfigService<WordsSettings>(services, configuration, WordsSettings.SectionName);
            AddConfigService<LearningSettings>(services, configuration, LearningSettings.SectionName);
            AddConfigService<PronunciationApiSettings>(services, configuration, PronunciationApiSettings.SectionName);
        }

        private static void AddConfigService<T>(
            ServiceCollection services,
            IConfiguration configuration,
            string sectionName) where T : class
        {
            services.Configure<T>(configuration.GetSection(sectionName));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<T>>().Value);
        }

        private static void AddPronunciationService(ServiceCollection services)
        {
            services.AddSingleton<IPronunciation, Pronunciation.Api.WebDictionary.PronunciationService>();
        }
    }
}
