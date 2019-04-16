using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Plugin.LocalNotifications;
using System.Windows.Input;
using Xamarin.Forms;
using KutyApp.Client.Common.Constants;
using System.Diagnostics;
using KutyApp.Client.Xam.Views;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Refit;
using Newtonsoft.Json;
using KutyApp.Client.Xam.Resources.Localization;
using System.Resources;
using System.Threading;

namespace KutyApp.Client.Xam.ViewModels
{
    public class LoginPopupPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApiService { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }

        public LoginPopupPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IKutyAppClientContext kutyAppClientContext)
            : base(navigationService)
        {
            EnvironmentApiService = environmentApi;
            KutyAppClientContext = kutyAppClientContext;
        }

        private string userName;
        private string password;
        private string errorMessage;
        private bool rememberMe;
        private bool hasError;
        private ICommand loginCommand;
        private ICommand registerCommand;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public bool HasError
        {
            get => hasError;
            set => SetProperty(ref hasError, value);
        }

        public bool RememberMe
        {
            get => rememberMe;
            set => SetProperty(ref rememberMe, value);
        }

        public ICommand LoginCommand =>
            loginCommand ?? (loginCommand = new Command(
                async () => await LoginAsync()));

        public ICommand RegisterCommand =>
            registerCommand ?? (registerCommand = new Command(
                async () => await NavigationService.NavigateAsync(nameof(RegisterPopupPage))));

        private async Task LoginAsync()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    IsBusy = true;
                    var response = await EnvironmentApiService.LoginAsync(new LoginDto { Email = UserName, Password = Password, RememberMe = RememberMe });

                    if (RememberMe)
                    {
                        await KutyAppClientContext.SaveSettingsAsync(response, null);
                    }
                    else
                    {
                        KutyAppClientContext.SetTemporaryApiKey(response);
                    }

                    //await NavigationService.NavigateAsync(nameof(Views.MainPage));
                    await NavigationService.ClearPopupStackAsync();
                }
                catch (ValidationApiException validationException)
                {
                    // handle validation here by using validationException.Content, 
                    // which is type of ProblemDetails according to RFC 7807
                }
                catch (ApiException exception)
                {
                    var error = JsonConvert.DeserializeObject<ErrorDto>(exception.Content);
                    HasError = true;
                    try
                    {
                        var ottva = Texts.ResourceManager.GetString(error.Message, Localization.Current.CurrentCultureInfo);
                        ErrorMessage = ottva;
                    }
                    catch (Exception)
                    {
                    }
               
                    // other exception handling
                }
                catch (System.Exception e)
                {
                }
            }

            IsBusy = false;
        }
    }
}
