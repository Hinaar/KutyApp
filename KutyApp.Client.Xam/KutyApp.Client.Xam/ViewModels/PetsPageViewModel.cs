using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private IPetRepository PetRepository { get; }
        private IPageDialogService PageDialogService { get; }
        public PetsPageViewModel(INavigationService navigationService, IPetRepository petRepository, IPageDialogService pageDialogService) : base(navigationService)
        {
            IsBusy = true;
            this.PetRepository = petRepository;
            this.PageDialogService = pageDialogService;
            Dogs = new ObservableCollection<Dog>();
        }

        private ICommand navigateToPetsDetailPage;
        private ICommand newPetCommand;
        private ICommand deletePetCommand;

        public ICommand NavigateToPetsDetailPage =>
                navigateToPetsDetailPage ?? (navigateToPetsDetailPage = new Command(
                    async param =>
                        //Debug.WriteLine(((Dog)param).Id));
                        await NavigationService.NavigateAsync(nameof(Views.PetDetailPage), new NavigationParameters { { ParameterKeys.PetId, (int)(param as Dog).Id } })));
        
        public ICommand NewPetCommand =>
                newPetCommand ?? (newPetCommand = new Command(
                    async () =>
                        //Debug.WriteLine(((Dog)param).Id));
                        await NavigationService.NavigateAsync(nameof(Views.PetDetailPage))));

        public ICommand DeletePetCommand =>
            deletePetCommand ?? (deletePetCommand = new Command(
                async param => await DeletePet(param as Dog)));

        private ObservableCollection<Dog> dogs;

        public ObservableCollection<Dog> Dogs
        {
            get { return dogs; }
            set { SetProperty(ref dogs, value); }
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = true;
            base.OnNavigatedTo(parameters);
            await LoadMyPetsAsync();
        }

        private async Task DeletePet (Dog dog)
        {
            IsBusy = true;
            await PetRepository.DeleteDogAsync(dog.Id);
            await LoadMyPetsAsync();
        }

        private async Task LoadMyPetsAsync()
        {
            IsBusy = true;
            var pets = await PetRepository.GetDogsAsync();
            Dogs = new ObservableCollection<Dog>(pets);
            IsBusy = false;
        }
    }
}
