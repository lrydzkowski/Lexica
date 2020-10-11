using Lexica.CLI.Interfaces;
using Lexica.CLI.Managers;
using Lexica.CLI.Models;
using Lexica.CLI.Models.Config;
using Lexica.EF.Services;
using Lexica.Words;
using Lexica.Words.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.CLI
{
    class Program
    {
        private static IServiceProvider? serviceProvider;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            try
            {
                int number123 = 1;
                string test123 = string.Format("Text number {0} created only for tests.", number123);
                string test321 = $"Text number {number123} created only for tests.";

                IfElsePatternMatching();
                return;

                //var set = new Set();
                //var setPath = new SetPath();
                //setPath.Namespace = "dir1";
                //setPath.Name = "set1";
                //var setInfo = new SetInfo();
                //setInfo.SetId = 1;
                //setInfo.Path = setPath;
                //set.SetsInfo = new List<SetInfo>() { setInfo };
                //set.Entries = new List<Entry>();
                //for (int i = 0; i < 5; i++)
                //{
                //    var entry = new Entry();
                //    entry.SetId = 1;
                //    entry.EntryId = i + 1;
                //    entry.Translations = new List<string>() { "translation" + i.ToString() };
                //    entry.Words = new List<string>() { "word" + i.ToString() };
                //    set.Entries.Add(entry);
                //}
                //var setService = new SetService();
                //var importer = new Importer(setService);
                //await importer.Import(set);

                return;

                // Dependency Injection.
                ConfigureServices();
                var orderManager = serviceProvider.GetService<IOrderManager>();
                var order = CreateOrder();
                await orderManager.Transmit(order);

                // Get embedded resource.
                var fileProvider = serviceProvider.GetService<IFileProvider>();
                var fileInfo = fileProvider.GetFileInfo("Assets/test.txt");
                string fileContent = null;
                if (fileInfo.Exists)
                {
                    using Stream stream = fileInfo.CreateReadStream();
                    using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    fileContent = reader.ReadToEnd();
                }
                var resourceLoader = serviceProvider.GetService<IResourceLoader>();
                fileContent = resourceLoader.GetEmbeddedResourceString(
                    Assembly.GetExecutingAssembly(),
                    "Assets.test.txt"
                );

                // Logging.
                log.Debug("Debug message");
                Loco.Tools.Log.Event.Write(new List<string> { "element1", "element2" }, "customLog");
                
                //var innerException = new Exception("Simple inner exception for testing.");
                //var ex = new Exception("Simple exception for testing.", innerException);
                var ex = new Exception("Simple exception for testing.");
                ex.Data.Add("lol-key", "lol-value");
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Loco.Tools.Log.Error.Write(ex, "errorLog");
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<IOrderSender, HttpOrderSender>();
            services.AddTransient<IOrderManager, OrderManager>();
            services.AddSingleton<IFileProvider>(x => new EmbeddedFileProvider(Assembly.GetExecutingAssembly()));
            services.AddSingleton<IResourceLoader, ResourceLoader>();

            AppSettings settings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build()
                .Get<AppSettings>();
            services.AddSingleton<AppSettings>(settings);

            serviceProvider = services.BuildServiceProvider();
        }

        private static Order CreateOrder()
        {
            return new Order
            {
                CustomerId = "12345",
                Date = new DateTime(),
                TotalAmount = 145,
                Items = new System.Collections.Generic.List<OrderItem>
                {
                    new OrderItem {
                        ItemId = "99999",
                        Quantity = 1,
                        Price = 145
                    }
                }
            };
        }

        static void IfElsePatternMatching()
        {
            Console.WriteLine("===If Else Pattern Matching ===/n");
            object testItem1 = 123;
            object testItem2 = "Hello";
            if (testItem1 is string myStringValue1)
            {
                Console.WriteLine($"{myStringValue1} is a string");
            }
            if (testItem1 is int myValue1)
            {
                Console.WriteLine($"{myValue1} is an int");
            }
            if (testItem2 is string)
            {
                Console.WriteLine($"{testItem2} is a string");
            }
            if (testItem2 is int myValue2)
            {
                Console.WriteLine($"{myValue2} is an int");
            }
            Console.WriteLine();
        }
    }
}
