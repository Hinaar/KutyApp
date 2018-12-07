using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.ServiceCollector.Interfaces
{
    public interface IPermissionManager
    {
        Task<bool> CheckAndGrantPermissionsAsyc(List<Permission> permissions);
    }
}
