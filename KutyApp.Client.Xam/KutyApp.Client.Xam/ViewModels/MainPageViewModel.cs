using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Views;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;
using Xamarin.Forms;

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
            IsLoggedIn = false;
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

            //try if token is still valid
            if (KutyAppClientContext.IsLoggedIn)
            {
                try
                {
                    await EnvironmentApi.AuthPingAsync();
                }
                catch (System.Exception e)
                {
                    KutyAppClientContext.RemoveApiKey();
                }
            }

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
        private ICommand navigateToProfilePage;
        private ICommand changeLanguage;
        private ICommand navigateToAdvertPage;
        private ICommand openPopupCommand;

        public ICommand NavigateToPetsPage =>
                navigateToPetsPage ?? (navigateToPetsPage = new Command(
                    async () =>
                        await NavigationService.NavigateAsync(nameof(Views.PetsPage))));

        public ICommand NavigateToPoisPage =>
                navigateToPoisPage ?? (navigateToPoisPage = new Command(
                    async () => 
                        await NavigationService.NavigateAsync(nameof(Views.PoisPage))));

        public ICommand NavigateToProfilePage =>
            navigateToProfilePage ?? (navigateToProfilePage = new Command(
                async () => await NavigationService.NavigateAsync(nameof(Views.ProfilePage))));

        public ICommand ChangeLanguage =>
            changeLanguage ?? (changeLanguage = new Command(
                async () =>
                    {
                        //already shows the required language
                        CurrentLanguage = IsEnglish ? Languages.En : Languages.Hu;
                        await NavigationService.NavigateAsync("app:///MainPage", animated: false);
                    }));

        public ICommand NavigateToAdvertPage =>
            navigateToAdvertPage ?? (navigateToAdvertPage = new Command(
                async () => await NavigationService.NavigateAsync(nameof(TempPage))));

        public ICommand OpenPopupCommand =>
            openPopupCommand ?? (openPopupCommand = new Command(
                async () => await NavigationService.NavigateAsync(nameof(LoginPopupPage))));
    }
}
