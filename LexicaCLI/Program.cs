using NLog;
using System;
using System.Collections.Generic;

namespace Lexica.CLI
{
    class Program
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                log.Debug("Debug message");
                //Loco.Tools.Log.Event.Write(new List<string> { "element1", "element2" }, "customLog");
                //var innerException = new Exception("Simple inner exception for testing.");
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
    }
}
