using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;

namespace KutyApp.Client.Services.ServiceCollector.Implementations
{
    public class MediaManager : IMediaManager
    {
        IPermissionManager PermissionManager { get; }

        public MediaManager(IPermissionManager pm)
        {
            PermissionManager = pm;
        }

        public async Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
        {
            MediaFile file = null;
            if (await PermissionManager.CheckAndGrantPermissionsAsyc(new List<Permission> { Permission.Camera, Permission.Storage }))
            {
                file = await CrossMedia.Current.PickPhotoAsync(options ?? new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                });
            }

            return file;
        }

        public async Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options = null)
        {
            MediaFile file = null;

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                throw new System.Exception("support");

            file = await CrossMedia.Current.TakePhotoAsync(options ?? new StoreCameraMediaOptions
            {
                Directory = "Test",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Rear
            });

            return file;
        }
    }
}
