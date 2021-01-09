using Lexica.Core.Config;
using Lexica.EF.Config.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lexica.EF.Config
{
    class Settings
    {
        public static AppSettings<Database> Get()
        {
            return AppSettings<Database>.GetSettings("database", Assembly.GetExecutingAssembly());
        }
    }
}
