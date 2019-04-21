using KutyApp.Client.Common.Constants;
using KutyApp.Client.Common.Enums;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Navigation;
using KutyApp.Client.Xam.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private IPetRepository PetRepository { get; }
        private IEnvironmentApiService EnvironmentApi { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }
        public ProfilePageViewModel(INavigationService navigationService, IPetRepository petRepository, IEnvironmentApiService environmentApi, IKutyAppClientContext kutyAppClientContext) : base(navigationService)
        {
            this.PetRepository = petRepository;
            this.EnvironmentApi = environmentApi;
            this.KutyAppClientContext = kutyAppClientContext;

            IsEnglish = CurrentLanguage.DisplayName == Languages.En.DisplayName || CurrentLanguage.DisplayName == Languages.Default.DisplayName;
            IsLoggedIn = KutyAppClientContext.IsLoggedIn;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (IsLoggedIn)
                await LoadSittersAsync();
        }

        private bool isEnglish;
        private bool isLoggedIn;
        private bool isListRefreshing;
        private ObservableCollection<UserDto> myPetSitters;

        public bool IsEnglish { get => isEnglish; set => SetProperty(ref isEnglish, value); }
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }
        public bool IsListRefreshing { get => isListRefreshing; set => SetProperty(ref isListRefreshing, value); }
        public ObservableCollection<UserDto> MyPetSitters { get => myPetSitters; set => SetProperty(ref myPetSitters, value); }

        private ICommand saveOfflineCommand;
        private ICommand changeLanguage;
        private ICommand openSitterPopupCommand;

        public ICommand SaveOfflineCommand =>
            saveOfflineCommand ?? (saveOfflineCommand = new Command(
                async () => await SaveOffline()));

        public ICommand ChangeLanguage =>
          changeLanguage ?? (changeLanguage = new Command(
              async () =>
              {
                    //already shows the required language
                  CurrentLanguage = IsEnglish ? Languages.En : Languages.Hu;
                  await KutyAppClientContext.SaveSettingsAsync(null, CurrentLanguage);
                  await NavigationService.NavigateAsync("app:///MainPage", animated: false);
              }));

        public ICommand OpenSitterPopupCommand =>
            openSitterPopupCommand ?? (openSitterPopupCommand = new Command(
                async (tappedSitter) => await NavigationService.NavigateAsync(nameof(PetSitterPopupPage),
                    new NavigationParameters {
                        {
                            nameof(NavigationHelper), new NavigationHelper
                            {
                                Action = tappedSitter is UserDto ? NavigationAction.Edit : NavigationAction.Add,
                                ParameterTypeName = nameof(UserDto),
                                Parameter = tappedSitter
                            }
                        } }
                )));

        private async Task SaveOffline()
        {
            IsBusy = true;

            var dtos = await EnvironmentApi.GetMyPetsAsync();
            if (dtos.Any())
            {
                await PetRepository.DeletePetsAsync();
                await PetRepository.SaveMyPetsAsync(dtos);
            }

            IsBusy = false;
        }

        private async Task LoadSittersAsync()
        {
            IsListRefreshing = true;
            try
            {
                var sitters = await EnvironmentApi.GetMySittersAsync();
                if (sitters.Any())
                    MyPetSitters = new ObservableCollection<UserDto>(sitters);
            }
            catch (Exception e)
            {

            }

            IsListRefreshing = false;
        }

    }
}
