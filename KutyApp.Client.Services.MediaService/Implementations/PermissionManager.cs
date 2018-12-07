using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.ServiceCollector.Implementations
{
    public class PermissionManager : IPermissionManager
    {
        public async Task<bool> CheckAndGrantPermissionsAsyc(List<Permission> permissions)
        {
            //await Task.Yield();
            Dictionary<Permission, PermissionStatus> statusDictionary = new Dictionary<Permission, PermissionStatus>();

            var permissionsToGrant = await CheckPermissionsAsync(permissions);
            if (permissionsToGrant.Any())
                statusDictionary = await RequestPermissionsAsync(permissions);

            foreach (var permission in permissions)
                if (statusDictionary[permission] != PermissionStatus.Granted)
                    //throw new Exception("Permission can not be granted");
                    return false;

            return true;
        }

        private async Task<List<Permission>> CheckPermissionsAsync(List<Permission> permissions)
        {
            List<Permission> missingPermissions = new List<Permission>();

            foreach (var permission in permissions)
                if (await CrossPermissions.Current.CheckPermissionStatusAsync(permission) != PermissionStatus.Granted)
                    missingPermissions.Add(permission);

            return missingPermissions;
        } 

        private async Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(List<Permission> permissions) =>
            await CrossPermissions.Current.RequestPermissionsAsync(permissions.ToArray());

    }
}
