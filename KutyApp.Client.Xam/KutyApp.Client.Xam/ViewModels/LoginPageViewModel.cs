using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Xam.Config;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPageDialogService DialogService { get; }

        public LoginPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IPageDialogService dialogService) : base(navigationService)
        {
            EnvironmentApi = environmentApi;
            DialogService = dialogService;
        }

        private string userName;
        private string password;

        private ICommand loginCommand;

        #region public properties
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public ICommand LoginCommand =>
            loginCommand ?? (loginCommand = new Command(
                async () => await LoginAsync()));
        #endregion

        private async Task LoginAsync()
        {
            if(!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    IsBusy = true;
                    var response = await EnvironmentApi.LoginAsync(new LoginDto { Email = UserName, Password = Password, RememberMe = false });
                    Configurations.ApiToken = response;
                    IsBusy = false;

                    await NavigationService.NavigateAsync(nameof(Views.MainPage));
                }
                catch (System.Exception e)
                {
                    await DialogService.DisplayAlertAsync("errror", e.Message, "ok");
                    //throw;
                }
            }

            IsBusy = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

    }
}
