using Lexica.EF.Services;
using Lexica.LearningMode.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lexica.EF.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddEFServices(this IServiceCollection services)
        {
            services.AddTransient<ILearningHistoryService, LearningHistoryService>();
        }
    }
}
