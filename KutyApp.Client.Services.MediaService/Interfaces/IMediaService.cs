using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.MediaService.Implementations
{
    public interface IMediaService
    {
        Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null);
        Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options = null);
    }
}
