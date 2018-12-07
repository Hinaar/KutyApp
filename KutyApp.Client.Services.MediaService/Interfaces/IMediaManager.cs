using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.ServiceCollector.Interfaces
{
    public interface IMediaManager
    {
        Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null);
        Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options = null);
    }
}
