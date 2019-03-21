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

namespace KutyApp.Client.Xam.ViewModels
{
    public class LoginPopupPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApiService { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }

        public LoginPopupPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IKutyAppClientContext kutyAppClientContext)
            : base(navigationService)
        {
            Title = "login vm title";
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

        //TODO: register page, nav
        public ICommand RegisterCommand =>
            registerCommand ?? (registerCommand = new Command(
                async () => await Task.Delay(0)));

        private async Task LoginAsync()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    IsBusy = true;
                    await Task.Delay(1000);
                    var response = await EnvironmentApiService.LoginAsync(new LoginDto { Email = UserName, Password = Password, RememberMe = false });

                    if (RememberMe)
                    {
                        await KutyAppClientContext.SaveSettingsAsync(response, null);
                    }
                    else
                    {
                        KutyAppClientContext.SetTemporaryApiKey(response);
                    }

                    await NavigationService.NavigateAsync(nameof(Views.MainPage));
                }
                catch (ValidationApiException validationException)
                {
                    // handle validation here by using validationException.Content, 
                    // which is type of ProblemDetails according to RFC 7807
                }
                catch (ApiException exception)
                {
                    //TODO: exc handling (json obj -> errormsg = msg)
                    // other exception handling
                }
                catch (System.Exception e)
                {
                    //TODO: bejelentk. hiba

                    //throw;
                }
            }

            IsBusy = false;
        }
    }
}
