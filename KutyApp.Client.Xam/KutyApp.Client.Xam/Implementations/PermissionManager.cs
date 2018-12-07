using KutyApp.Client.Xam.Interfaces;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KutyApp.Client.Xam.Implementations
{
    public class PermissionManager : IPermissionManager
    {
        public Task<bool> CheckAndGrantPermissionsAsyc(List<Permission> permissions)
        {
            throw new NotImplementedException();
        }

        private async Task<List<Permission>> CheckPermissionsAsync(List<Permission> permissions)
        {
            List<Permission> missingPermissions = new List<Permission>();

            foreach (var permission in permissions)
                if (await CrossPermissions.Current.CheckPermissionStatusAsync(permission) != PermissionStatus.Granted)
                    missingPermissions.Add(permission);

            return missingPermissions;
        } 

        private async Task<List<Permission>> RequestPermissionsAsync(List<Permission> permissions)
        {
            var tmp = await CrossPermissions.Current.RequestPermissionsAsync(permissions.ToArray());
            throw new NotImplementedException();
        }

    }
}
