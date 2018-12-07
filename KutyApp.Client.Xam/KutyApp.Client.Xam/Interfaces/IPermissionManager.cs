using Plugin.Permissions.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutyApp.Client.Xam.Interfaces
{
    public interface IPermissionManager
    {
        Task<bool> CheckAndGrantPermissionsAsyc(List<Permission> permissions);
    }
}
