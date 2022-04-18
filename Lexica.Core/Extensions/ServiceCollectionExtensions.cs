using Lexica.Core.Services;
using Microsoft.Extensions.DependencyInjection;

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