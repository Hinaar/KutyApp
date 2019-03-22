using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KutyApp.Client.Services.ServiceCollector.Implementations
{
    public class KutyAppClientContext : IKutyAppClientContext
    {
        public bool IsLoggedIn { get; set; } = false;
        public string ApiKey { get; protected set; } = string.Empty;
        public CultureInfo SavedLanguage { get; set; } = Languages.Default;
        private bool IsSettingsLoaded { get; set; } = false;

        public void SetTemporaryApiKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                ApiKey = key;
                IsLoggedIn = true;
            }
        }

        public async Task LoadSettingsAsync()
        {
            if (!IsSettingsLoaded)
            {
                string apiKey = string.Empty;
                try
                {
                    apiKey = await SecureStorage.GetAsync($"{nameof(KutyApp)}.{nameof(ApiKey)}");
                }
                catch (System.Exception)
                {
                    apiKey = Preferences.Get($"{nameof(KutyApp)}.{nameof(ApiKey)}", string.Empty);
                }
                if (!string.IsNullOrEmpty(apiKey))
                {
                    ApiKey = apiKey;
                    IsLoggedIn = true;
                }

                string cultureString = Preferences.Get($"{nameof(KutyApp)}.{nameof(SavedLanguage)}", string.Empty);
                if (!string.IsNullOrEmpty(cultureString))
                {
                    try
                    {
                        SavedLanguage = new CultureInfo(cultureString);
                    }
                    catch (System.Exception)
                    {
                        SavedLanguage = Languages.Default;
                    }
                }
                else
                    SavedLanguage = Languages.Default;

                IsSettingsLoaded = true;
            }
            
        }

        public void RemoveApiKey()
        {
            SecureStorage.Remove($"{nameof(KutyApp)}.{nameof(ApiKey)}");
            Preferences.Remove($"{nameof(KutyApp)}.{nameof(ApiKey)}");
        }

        public async Task<string> ReturnApiKeyAsync() => await Task.FromResult(ApiKey);

        public async Task SaveSettingsAsync(string apiKey, CultureInfo language)
        {
            if (!string.IsNullOrEmpty(apiKey))
            {
                ApiKey = apiKey;
                IsLoggedIn = true;

                try
                {
                    await SecureStorage.SetAsync($"{nameof(KutyApp)}.{nameof(ApiKey)}", apiKey);
                }
                catch (System.Exception)
                {
                    Preferences.Set($"{nameof(KutyApp)}.{nameof(ApiKey)}", apiKey);
                }
            }

            if(language != null)
            {
                SavedLanguage = language;

                Preferences.Set($"{nameof(KutyApp)}.{nameof(SavedLanguage)}", language.Name);
            }

        }
    }
}
