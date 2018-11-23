using KutyApp.Client.Services.LocalRepository.Entities.Models;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetsPageViewModel : ViewModelBase
    {
        private IPetRepository PetRepository { get; }
        private IPageDialogService PageDialogService { get; }
        public PetsPageViewModel(INavigationService navigationService, IPetRepository petRepository, IPageDialogService pageDialogService) : base(navigationService)
        {
            this.PetRepository = petRepository;
            this.PageDialogService = pageDialogService;
            Dogs = new ObservableCollection<Dog>(new List<Dog> { new Dog { Name = "nevem he"} });
            string s = string.Empty;
        }

        private ObservableCollection<Dog> dogs;

        public ObservableCollection<Dog> Dogs
        {
            get { return dogs; }
            set { SetProperty(ref dogs, value); }
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await Task.Yield();
            await LoadMyPetsAsync();
            base.OnNavigatedTo(parameters);
        }

        private async Task LoadMyPetsAsync()
        {
            var pets = await PetRepository.GetDogsAsync();
            Dogs = new ObservableCollection<Dog>(pets);
        }
    }
}
