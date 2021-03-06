using System.IO;
using System.Reflection;

namespace Lexica.CLI.Core.Services
{
    class LocationService : IService
    {
        public string GetExecutingAssemblyLocation()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(location) ?? location;
        }
    }
}
