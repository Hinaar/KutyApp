using KutyApp.Client.Common.Constants;
using KutyApp.Client.Services.LocalRepository.Entities.Enums;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
        #endregion

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await Task.Yield();
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
        }
    }
}
