using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
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
            IsEnglish = CurrentLanguage == Languages.En || CurrentLanguage == Languages.Default;
            IsLoggedIn = KutyAppClientContext.IsLoggedIn;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        private bool isEnglish;
        private bool isLoggedIn;
        public bool IsEnglish { get => isEnglish; set => SetProperty(ref isEnglish, value); }
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }


        private ICommand saveOfflineCommand;
        private ICommand changeLanguage;

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
    }
}
