using KutyApp.Services.Environment.Bll.Context;
using KutyApp.Services.Environment.Bll.Interfaces;
using KutyApp.Services.Environment.Bll.Managers;
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
            services.AddTransient<IPetManager, PetManager>();
            services.AddTransient<IDataManager, DataManager>();
            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddScoped<IKutyAppContext, KutyAppContext>();

            return services;
        }
    }
}
