using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private ICommand refreshListCommand;

        public ICommand NavigateToPetsDetailPage =>
                navigateToPetsDetailPage ?? (navigateToPetsDetailPage = new Command(
                    async param =>
                        await NavigationService.NavigateAsync(nameof(Views.PetDetailPage), new NavigationParameters { { ParameterKeys.PetId, (int)(param as Dog).Id } })));

        public ICommand NewPetCommand =>
                newPetCommand ?? (newPetCommand = new Command(
                    async () =>
                        await NavigationService.NavigateAsync(nameof(Views.PetDetailPage))));

        public ICommand DeletePetCommand =>
            deletePetCommand ?? (deletePetCommand = new Command(
                async param => await DeletePet(param as Dog)));

        public ICommand RefreshListCommand =>
             refreshListCommand ?? (refreshListCommand = new Command(
                 async () => await LoadMyPetsAsync(true)));

        private ObservableCollection<Dog> dogs;

        public ObservableCollection<Dog> Dogs
        {
            get { return dogs; }
            set { SetProperty(ref dogs, value); }
        }

        private bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await LoadMyPetsAsync();
        }

        private async Task DeletePet(Dog dog)
        {
            await PetRepository.DeleteDogAsync(dog.Id);
            await LoadMyPetsAsync();
        }

        private async Task LoadMyPetsAsync(bool isRefresh = false)
        {
            if (isRefresh)
                IsRefreshing = true;
            else
                IsBusy = true;

            var pets = await PetRepository.GetDogsAsync();
            Dogs = new ObservableCollection<Dog>(pets);
            if (!Dogs.Any())
                Dogs.Add(new Dog { Name = "My First Pet", BirthDate = DateTime.Now.AddYears(-1) });


            if (isRefresh)
                IsRefreshing = false;
            else IsBusy = false;
        }
    }
}
