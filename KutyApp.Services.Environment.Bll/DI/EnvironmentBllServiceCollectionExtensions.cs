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
            services.AddTransient<IAdvertManager, AdvertManager>();
            services.AddTransient<IDatabaseManager, DatabaseManager>();
            services.AddTransient<IDataManager, DataManager>();
            services.AddTransient<ILocationManager, LocationManager>();
            services.AddTransient<IPetManager, PetManager>();
            services.AddTransient<IPoiManager, PoiManager>();
            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddScoped<IKutyAppContext, KutyAppContext>();

            return services;
        }
    }
}
