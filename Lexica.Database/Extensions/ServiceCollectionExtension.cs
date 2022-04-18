using System.Data;
using System.Data.SQLite;
using Lexica.Database.Services;
using Lexica.Learning.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lexica.Database.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(_ => new SQLiteConnection(@"Data Source=.\\lexica.db"));
            services.AddTransient<ILearningHistoryService, LearningHistoryService>();
        }
    }
}