using KutyApp.Services.Environment.Api.Managers;
using KutyApp.Services.Environment.Bll.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KutyApp.Services.Environment.Api.DI
{
    public static class EnvironmentApiServiceCollectionExtensions
    {
        public static IServiceCollection AddApiManagers(this IServiceCollection services)
        {
            services.AddTransient<IAuthManager, AuthManager>();

            return services;
        }
    }
}
