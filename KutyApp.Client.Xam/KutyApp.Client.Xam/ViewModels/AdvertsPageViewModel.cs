using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Navigation;
using KutyApp.Client.Xam.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class AdvertsPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }

        public AdvertsPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApiService, IKutyAppClientContext kutyAppClientContext) : base(navigationService)
        {
            EnvironmentApi = environmentApiService;
            KutyAppClientContext = kutyAppClientContext;
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (KutyAppClientContext.IsLoggedIn)
            {
                IsLoggedIn = true;
                await LoadAdverts();
            }
        }

        private string keyWord;
        private ObservableCollection<AdvertDto> adverts;
        private bool isRefreshing;
        private bool isLoggedIn;

        public string KeyWord { get => keyWord; set => SetProperty(ref keyWord, value); }
        public ObservableCollection<AdvertDto> Adverts { get => adverts; set => SetProperty(ref adverts, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }
        public bool IsLoggedIn { get => isLoggedIn; set => SetProperty(ref isLoggedIn, value); }

        private ICommand searchCommand;
        private ICommand openAdvertCommand;

        public ICommand SearchCommand =>
            searchCommand ?? (searchCommand = new Command(
                async () => await LoadAdverts()));

        public ICommand OpenAdvertCommand =>
            openAdvertCommand ?? (openAdvertCommand = new Command(
                async (item) =>
                        await NavigationService.NavigateAsync(nameof(AdvertisementPopupPage), new NavigationParameters
                        {
                            {
                                nameof(NavigationHelper),
                                new NavigationHelper
                                {
                                    Action = item is AdvertDto ? Common.Enums.NavigationAction.Edit : Common.Enums.NavigationAction.Add,
                                    Parameter = item,
                                    ParameterTypeName = nameof(AdvertDto)
                                }
                            }
                        })
                    ));

        private async Task LoadAdverts()
        {
            IsBusy = IsRefreshing = true;

            var adverts = await EnvironmentApi.ListLatestAdvertisementsAsync(new SearchAdvertDto { KeyWord = KeyWord });

            Adverts = new ObservableCollection<AdvertDto>((adverts ?? Enumerable.Empty<AdvertDto>()).ToList());

            IsBusy = IsRefreshing = false;
        }
    }
}
