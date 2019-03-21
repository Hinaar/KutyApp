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
using KutyApp.Client.Services.ServiceCollector.Interfaces;

namespace KutyApp.Client.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPageDialogService PageDialogService { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }
        private bool LoginAsked { get; set; }

        public MainPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IPetRepository petRepository, IPageDialogService dialogService, IKutyAppClientContext kutyAppClientContext)
            : base(navigationService)
        {
            this.EnvironmentApi = environmentApi;
            this.PageDialogService = dialogService;
            KutyAppClientContext = kutyAppClientContext;
            IsEnglish = CurrentLanguage == Languages.En || CurrentLanguage == Languages.Default;
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            await KutyAppClientContext.LoadSettingsAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            //TODO: xamarin essentials
            base.OnNavigatedTo(parameters);

            this.IsLoggedIn = KutyAppClientContext.IsLoggedIn;

            if (!KutyAppClientContext.IsLoggedIn && !LoginAsked)
            {
                await NavigationService.NavigateAsync(nameof(LoginPopupPage));
                LoginAsked = true;
            }
        }

        private bool isEnglish;
        private bool isLoggedIn;
        public bool IsEnglish { get => isEnglish; set => SetProperty(ref isEnglish, value); }
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }

        private ICommand navigateToPetsPage;
        private ICommand navigateToPoisPage;
        private ICommand changeLanguage;

        public ICommand NavigateToPetsPage =>
                navigateToPetsPage ?? (navigateToPetsPage = new Command(
                    async () =>
                        await NavigationService.NavigateAsync(nameof(Views.PetsPage))));

        public ICommand NavigateToPoisPage =>
                navigateToPoisPage ?? (navigateToPoisPage = new Command(
                    async () => 
                        await NavigationService.NavigateAsync(nameof(Views.PoisPage))));

        public ICommand ChangeLanguage =>
            changeLanguage ?? (changeLanguage = new Command(
                async () =>
                    {
                        //TODO: visszalepes utan lefut a nav
                        //already shows the required language
                        CurrentLanguage = IsEnglish ? Languages.En : Languages.Hu;
                        await NavigationService.NavigateAsync("app:///MainPage", animated: false);
                    }));

        private ICommand openPopupCommand;

        public ICommand OpenPopupCommand =>
            openPopupCommand ?? (openPopupCommand = new Command(
                async () => await NavigationService.NavigateAsync(nameof(LoginPopupPage))));
    }
}
