using Lexica.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreLibraryServices(this IServiceCollection services)
        {
            services.AddSingleton<UrlService>();
        }
    }
}
