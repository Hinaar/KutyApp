using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace KutyApp.Client.Services.MediaService.Implementations
{
    public class MediaService : IMediaService
    {
        public async Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
