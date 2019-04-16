using KutyApp.Services.Environment.Bll.Configuration;
using KutyApp.Services.Environment.Bll.Interfaces;
using KutyApp.Services.Environment.Bll.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutyApp.Services.Environment.Bll.Managers
{
    public class DataManager : IDataManager
    {
        private FileSettings FileSettings { get; }

        public DataManager(IOptions<FileSettings> fileSettings)
        {
            FileSettings = fileSettings.Value;
        }

        public void DeleteFile(string uri)
        {
            string fullPath = Path.GetFullPath(uri);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public Task DeleteUnreferencedImagesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetImageAsync(string path)
        {
            if(string.IsNullOrEmpty(path))
                throw new ArgumentNullException();

            string fullPath = Path.GetFullPath(Path.Combine(FileSettings.RootDirectory, path));
            if (File.Exists(fullPath))
                return await File.ReadAllBytesAsync(fullPath);
            else
                throw new Exception(ExceptionMessages.NotFound);
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string filename = GetFilename(file.FileName);

            string path = Path.Combine(FileSettings.ImagesPath, filename);
            string directory = Path.GetDirectoryName(Path.GetFullPath(Path.Combine(FileSettings.RootDirectory, path)));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (FileStream fileStream = new FileStream(Path.Combine(FileSettings.RootDirectory, path), FileMode.Create))
                await file.CopyToAsync(fileStream);

            return path;
        }

        private string GetFilename(string filename)
        {
            string sourceFilename = Path.GetFileNameWithoutExtension(filename);
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
                sourceFilename = sourceFilename.Replace(c, '_');

            string prefix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string targetFilename = $"{prefix}_{sourceFilename}";

            string extension = Path.GetExtension(filename);
            return Path.ChangeExtension(targetFilename, extension);
        }

        private string GetFullDirectoryPath(string path) =>
           Path.GetFullPath(Path.GetDirectoryName(Path.Combine(FileSettings.RootDirectory, path)));
    }
}
