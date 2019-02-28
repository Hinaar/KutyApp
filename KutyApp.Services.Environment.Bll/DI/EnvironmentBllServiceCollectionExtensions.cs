using KutyApp.Services.Environment.Bll.Managers;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using KutyApp.Services.Environment.Bll.Interfaces.Context;
using KutyApp.Services.Environment.Bll.Context;

namespace KutyApp.Services.Environment.Bll.DI
{
    public static class EnvironmentBllServiceCollectionExtensions
    {
        public static IServiceCollection AddBllManagers(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseManager, DatabaseManager>();
            services.AddTransient<IPoiManager, PoiManager>();
            services.AddTransient<ILocationManager, LocationManager>();

            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddScoped<IKutyAppContext, KutyAppContext>();

            return services;
        }
    }
}
