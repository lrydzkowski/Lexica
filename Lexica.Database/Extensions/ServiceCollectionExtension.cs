﻿using Lexica.Database.Services;
using Lexica.Learning.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SQLite;

namespace Lexica.Database.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(db => new SQLiteConnection(@"Data Source=.\\lexica.db"));
            services.AddTransient<ILearningHistoryService, LearningHistoryService>();
        }
    }
}
