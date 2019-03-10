using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Interfaces
{
    public interface IDataManager
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task DeleteUnreferencedImagesAsync();
        Task<byte[]> GetImageAsync(string path);
        void DeleteFile(string path);
    }
}
