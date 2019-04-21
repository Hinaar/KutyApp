using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Navigation;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PetSitterPopupPageViewModel : ViewModelBase
    {
        private IKutyAppClientContext KutyAppClientContext { get; }
        private IEnvironmentApiService EnvironmentApi { get; }

        public PetSitterPopupPageViewModel(INavigationService navigationService, IKutyAppClientContext kutyAppClientContext, IEnvironmentApiService apiService)
            : base(navigationService)
        {
            KutyAppClientContext = kutyAppClientContext;
            EnvironmentApi = apiService;
        }

        private bool isAdd;
        private bool isListLoading;
        private string keyWord;
        private ObservableCollection<UserDto> availableSitters;
        private UserDto selectedSitter;

        public bool IsAdd { get => isAdd; set => SetProperty(ref isAdd, value); }
        public bool IsListLoading { get => isListLoading; set => SetProperty(ref isListLoading, value); }
        public string KeyWord { get => keyWord; set => SetProperty(ref keyWord, value); }
        public ObservableCollection<UserDto> AvailableSitters { get => availableSitters; set => SetProperty(ref availableSitters, value); }
        public UserDto SelectedSitter
        {
            get => selectedSitter;
            set
            {
                SetProperty(ref selectedSitter, value);
                RefreshSelectedUserUI();
            }
        }

        private void RefreshSelectedUserUI()
        {
            RaisePropertyChanged(nameof(SelectedSitter.Email));
            RaisePropertyChanged(nameof(SelectedSitter.PhoneNumber));
            RaisePropertyChanged(nameof(SelectedSitter.UserName));
        }

        private ICommand searchCommand;
        private ICommand addPetSitterCommand;
        private ICommand removePetSitterCommand;

        public ICommand SearchCommand =>
            searchCommand ?? (searchCommand = new Command(
                async () =>
                {
                    IsListLoading = true;

                    var sitters = await EnvironmentApi.ListAvailableSittersAsync(KeyWord ?? string.Empty);
                    AvailableSitters = new ObservableCollection<UserDto>(sitters);

                    IsListLoading = false;
                }));

        public ICommand AddPetSitterCommand =>
            addPetSitterCommand ?? (addPetSitterCommand = new Command(
                async () => await EnvironmentApi.AddPetSitterAsync(new AddOrRemovePetSitterDto { UserName = SelectedSitter.UserName})));

        public ICommand RemovePetSitterCommand =>
            removePetSitterCommand ?? (removePetSitterCommand = new Command(
                async () => await EnvironmentApi.RemovePetSitterAsync(new AddOrRemovePetSitterDto { UserName = SelectedSitter.UserName })));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(NavigationHelper)))
            {
                var param = (NavigationHelper)parameters[nameof(NavigationHelper)];

                switch (param.Action)
                {
                    case Common.Enums.NavigationAction.Add:
                        IsAdd = true;
                        break;
                    case Common.Enums.NavigationAction.Edit:
                        var user = param.Parameter as UserDto;

                        SelectedSitter = user;
                        break;
                    default:
                        break;
                }

                //originalHabit = param.Parameter as HabitDto;
                //SetUIValues(originalHabit);
            }

            base.OnNavigatedTo(parameters);
        }

       
    }
}
