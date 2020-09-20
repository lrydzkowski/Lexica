using Lexica.CLI.Interfaces;
using Lexica.CLI.Managers;
using Lexica.CLI.Models;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lexica.CLI
{
    class Program
    {
        private static IServiceProvider serviceProvider;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            try
            {
                // Dependency Injection.
                ConfigureServices();
                var orderManager = serviceProvider.GetService<IOrderManager>();
                var order = CreateOrder();
                await orderManager.Transmit(order);

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
    }
}
