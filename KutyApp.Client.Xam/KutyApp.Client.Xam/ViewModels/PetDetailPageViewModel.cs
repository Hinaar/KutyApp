using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.LocalRepository.Entities.Enums;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetDetailPageViewModel : ViewModelBase
    {
        private IPetRepository PetRepository { get; }
        private IMediaManager MediaManager { get; }
        private Dog Pet { get; set; }

        public PetDetailPageViewModel(INavigationService navigationService, IPetRepository petRepository, IMediaManager mediaManager) : base(navigationService)
        {
            IsBusy = true;
            PetRepository = petRepository;
            MediaManager = mediaManager;
        }

        private string name;
        private string chipNumber;
        private Gender gender;
        public IEnumerable<Gender> GenderValues => Enum.GetValues(typeof(Gender)).Cast<Gender>();
        private DateTime birthDate;
        private int? age;
        private int id;
        private string imagePath;

        private ICommand addOrEditPetCommand;
        private ICommand deletePetCommand;
        private ICommand takePhotoCommand;
        private ICommand pickPhotoCommand;

        #region Public Properties
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string ChipNumber
        {
            get { return chipNumber; }
            set { SetProperty(ref chipNumber, value); }
        }

        public Gender Gender
        {
            get { return gender; }
            set { SetProperty(ref gender, value); }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set { SetProperty(ref birthDate, value); }
        }

        public int? Age
        {
            get { return age; }
            set { SetProperty(ref age, value); }
        }

        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public string ImagePath
        {
            get { return imagePath; }
            set { SetProperty(ref imagePath, value); }
        }
        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKeys.PetId))
                await LoadPet((int)parameters[ParameterKeys.PetId]);

            IsBusy = false;
        }

        private async Task LoadPet(int id)
        {
            Pet = await PetRepository.GetDogByIdAsync(id);
            Name = Pet.Name;
            ChipNumber = Pet.ChipNumber;
            Gender = Pet.Gender;
            BirthDate = Pet.BirthDate ?? DateTime.MinValue;
            Age = Pet.Age;
            Id = Pet.Id;
            ImagePath = Pet.ImagePath;
        }

        public ICommand AddOrEditPetCommand =>
                 addOrEditPetCommand ?? (addOrEditPetCommand = new Command(
                    async () =>
                    {
                        var dog = await PetRepository.AddOrEditDogAsync(Pet ?? new Dog {Name = Name, ChipNumber = chipNumber, Gender = Gender, BirthDate = BirthDate, ImagePath = ImagePath });
                        if (dog.Id != 0)
                            await NavigationService.NavigateAsync(nameof(Views.PetsPage));
                    }));

        public ICommand DeletePetCommand =>
                deletePetCommand ?? (deletePetCommand = new Command(
                    async () =>
                    {
                        await PetRepository.DeleteDogAsync(id);
                        await NavigationService.NavigateAsync(nameof(Views.PetsPage));
                    }));


        public ICommand TakePhotoCommand =>
            takePhotoCommand ?? (takePhotoCommand = new Command(
                async () => await TakePhotoAsync()));

        public ICommand PickPhotoCommand =>
            pickPhotoCommand ?? (pickPhotoCommand = new Command(
                async () => await PickPhotoAsync()));

        private async Task PickPhotoAsync()
        {
            var file = await MediaManager.PickPhotoAsync();
            if(file != null)
            {
                //TODO: save picked image into private library

                Pet.ImagePath = file.Path;
                ImagePath = file.Path;
                file.Dispose();
            }
        }

        private async Task TakePhotoAsync()
        {
            var file = await MediaManager.TakePhotoAsync();

            if (file != null)
            {
                Pet.ImagePath = file.Path;
                ImagePath = file.Path;
                file.Dispose();
            }
        }
    }
}
