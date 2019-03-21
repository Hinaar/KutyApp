using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace KutyApp.Client.Services.ServiceCollector.Interfaces
{
    public interface IKutyAppClientContext
    {
        bool IsLoggedIn { get; set; }
        string ApiKey { get; }
        Task<string> ReturnApiKeyAsync();
        Task LoadSettingsAsync();
        CultureInfo SavedLanguage { get; set; }
        Task SaveSettingsAsync(string ApiKey, CultureInfo language);
        void RemoveApiKey();
        void SetTemporaryApiKey(string key);
    }
}
