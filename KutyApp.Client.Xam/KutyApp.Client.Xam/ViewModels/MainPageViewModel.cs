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

namespace KutyApp.Client.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPetRepository PetRepository { get; }
        private IPageDialogService PageDialogService { get; }

        public MainPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IPetRepository petRepository, IPageDialogService dialogService)
            : base(navigationService)
        {
            this.EnvironmentApi = environmentApi;
            this.PetRepository = petRepository;
            this.PageDialogService = dialogService;
            Title = "Main a Page";
        }

        private ICommand navigateToPetsPage;
        private ICommand navigateToPoisPage;

        public ICommand NavigateToPetsPage { get {
                return navigateToPetsPage ?? (navigateToPetsPage = new Command(
                    async () =>
                        await NavigationService.NavigateAsync(nameof(Views.PetsPage))));
            }
        }

        public ICommand NavigateToPoisPage { get {
                return navigateToPoisPage ?? (navigateToPoisPage = new Command(
                    async () => 
                        await NavigationService.NavigateAsync(nameof(Views.PoisPage)))); 
                }
        }
        
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //TODO: xamarin essentials

            base.OnNavigatedTo(parameters);
        }

    }
}
