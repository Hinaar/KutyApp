using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.LocalRepository.Entities.Enums;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetDetailPageViewModel : ViewModelBase
    {
        private IPetRepository PetRepository { get; }

        public PetDetailPageViewModel(INavigationService navigationService, IPetRepository petRepository) : base(navigationService)
        {
            this.PetRepository = petRepository;
        }

        private string name;
        private string chipNumber;
        private Gender gender;
        public IEnumerable<Gender> GenderValues => Enum.GetValues(typeof(Gender)).Cast<Gender>();
        private DateTime birthDate;
        private int? age;
        private int id;

        private ICommand addOrEditPetCommand;
        private ICommand deletePetCommand;

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
        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKeys.PetId))
                await LoadPet((int)parameters[ParameterKeys.PetId]);
        }

        private async Task LoadPet(int id)
        {
            var pet = await PetRepository.GetDogByIdAsync(id);
            Name = pet.Name;
            ChipNumber = pet.ChipNumber;
            Gender = pet.Gender;
            BirthDate = pet.BirthDate ?? DateTime.MinValue;
            Age = pet.Age;
            Id = pet.Id;
        }

        public ICommand AddOrEditPetCommand { get {
                return addOrEditPetCommand ?? (addOrEditPetCommand = new Command(
                    async () =>
                    {
                        var dog = await PetRepository.AddOrEditDogAsync(new Dog { Id = this.Id, Name = this.Name, ChipNumber = this.chipNumber, BirthDate = BirthDate ==  DateTime.MinValue ? (DateTime?)null : BirthDate, Gender = this.Gender });
                        if (dog.Id != 0)
                            await NavigationService.NavigateAsync(nameof(Views.PetsPage));
                    }));
            }
        }

        public ICommand DeletePetCommand { get {
                return deletePetCommand ?? (deletePetCommand = new Command(
                    async () =>
                    {
                        await PetRepository.DeleteDogAsync(id);
                        await NavigationService.NavigateAsync(nameof(Views.PetsPage));
                    }));
            }
        }


    }
}
