using KutyApp.Services.Environment.Bll.Managers;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
