using Lexica.EF.Services;
using Lexica.MaintainingMode.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.EF.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddEFServices(this IServiceCollection services)
        {
            services.AddTransient<IMaintainingHistoryService, MaintainingHistoryService>();
        }
    }
}
