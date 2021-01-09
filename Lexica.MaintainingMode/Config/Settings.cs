using Lexica.Core.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lexica.MaintainingMode.Config
{
    class Settings
    {
        public static AppSettings<Config.Models.Maintaining> Get()
        {
            return AppSettings<Config.Models.Maintaining>.GetSettings("maintaining", Assembly.GetExecutingAssembly());
        }
    }
}
