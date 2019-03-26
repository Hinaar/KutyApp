using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Async;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private IPetRepository PetRepository { get; }
        private IPageDialogService PageDialogService { get; }
        private IEnvironmentApiService EnvironmentApi { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }
        public PetsPageViewModel(INavigationService navigationService, IPetRepository petRepository, IPageDialogService pageDialogService, IEnvironmentApiService environmentApi, IKutyAppClientContext kutyAppClientContext) : base(navigationService)
        {
            this.PetRepository = petRepository;
            this.PageDialogService = pageDialogService;
            this.EnvironmentApi = environmentApi;
            this.KutyAppClientContext = kutyAppClientContext;
            //Pets = new ObservableCollection<PetsListItemViewModel>();
        }

        private ICommand navigateToPetsDetailPage;
        private ICommand newPetCommand;
        private ICommand deletePetCommand;
        private ICommand refreshListCommand;

        public ICommand NavigateToPetsDetailPage =>
                navigateToPetsDetailPage ?? (navigateToPetsDetailPage = new Command(
                    async param =>
                        await NavigationService.NavigateAsync(nameof(Views.PetDetailPage), new NavigationParameters { { ParameterKeys.PetId, (int)(param as PetsListItemViewModel).PetDto.Id } })));

        public ICommand NewPetCommand =>
                newPetCommand ?? (newPetCommand = new Command(
                    async () =>
                        await NavigationService.NavigateAsync(nameof(Views.PetDetailPage))));

        public ICommand DeletePetCommand =>
            deletePetCommand ?? (deletePetCommand = new Command(
                async param => await DeletePet((param as PetsListItemViewModel).PetDto)));

        public ICommand RefreshListCommand =>
             refreshListCommand ?? (refreshListCommand = new Command(
                 async () => await LoadMyPetsAsync(true)));

        private ObservableCollection<PetsListItemViewModel> pets;

        public ObservableCollection<PetsListItemViewModel> Pets
        {
            get { return pets; }
            set { SetProperty(ref pets, value); }
        }

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }
        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await LoadMyPetsAsync();
        }

        private async Task DeletePet(PetDto pet)
        {
            if (KutyAppClientContext.IsLoggedIn)
            {
                await EnvironmentApi.DeletePetAsync(pet.Id);
                await LoadMyPetsAsync();
            }
            else
            {
                await PageDialogService.DisplayAlertAsync("warning", "csak online lehet torolni", "OK");
                //await PetRepository.DeleteDogAsync(pet.Id);
            }
        }

        private async Task LoadMyPetsAsync(bool isRefresh = false)
        {
            if (isRefresh)
                IsRefreshing = true;
            else
                IsBusy = true;

            IEnumerable<PetDto> pets;

            if (KutyAppClientContext.IsLoggedIn)
                pets = await EnvironmentApi.GetMyPetsAsync();
            else
                pets = await PetRepository.GetMyPetsAsync();

            //TODO: ures listanal a parallel elszal
            if (pets.Any())
            {
                Pets = new ObservableCollection<PetsListItemViewModel>(pets.Select(p => new PetsListItemViewModel(EnvironmentApi, KutyAppClientContext,p)));

                if (isRefresh)
                    IsRefreshing = false;
                else IsBusy = false;
                await Pets.ParallelForEachAsync(async p => await p.LoadImageAsync(), maxDegreeOfParalellism: 4);
            }

            if (isRefresh)
                IsRefreshing = false;
            else IsBusy = false;
        }
    }
}
