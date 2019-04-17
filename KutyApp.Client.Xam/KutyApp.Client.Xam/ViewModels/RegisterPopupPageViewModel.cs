using KutyApp.Client.Services.ClientConsumer.Dtos;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Resources.Localization;
using Newtonsoft.Json;
using Prism.Navigation;
using Refit;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.ViewModels
{
    public class RegisterPopupPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApiService { get; }
        private IKutyAppClientContext KutyAppClientContext { get; }

        public RegisterPopupPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IKutyAppClientContext kutyAppClientContext)
            : base(navigationService)
        {
            Title = "register vm title";
            EnvironmentApiService = environmentApi;
            KutyAppClientContext = kutyAppClientContext;
        }

        private string userName;
        private string email;
        private string password;
        private string passwordConfirm;
        private string phoneNumber;
        private string errorMessage;
        private bool hasError;
        private ICommand registerCommand;

        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string PasswordConfirm
        {
            get => passwordConfirm;
            set => SetProperty(ref passwordConfirm, value);
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public bool HasError
        {
            get => hasError;
            set => SetProperty(ref hasError, value);
        }

        public ICommand RegisterCommand =>
            registerCommand ?? (registerCommand = new Command(
                async () => await RegisterAsync()));

        private async Task RegisterAsync()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                try
                {
                    IsBusy = true;
                    var response = await EnvironmentApiService.RegisterAsync(new RegisterDto { Email = UserName, Password = Password, PasswordConfirm = PasswordConfirm });

                    KutyAppClientContext.SetTemporaryApiKey(response);

                    await NavigationService.ClearPopupStackAsync();
                }
                catch (ValidationApiException validationException)
                {
                    // handle validation here by using validationException.Content, 
                    // which is type of ProblemDetails according to RFC 7807
                }
                catch (ApiException exception)
                {
                    var error = JsonConvert.DeserializeObject<ErrorDto>(exception.Content);
                    HasError = true;
                    try
                    {
                        ErrorMessage = Texts.ResourceManager.GetString(error.Message, Localization.Current.CurrentCultureInfo) ?? Texts.SERVERERROR;
                    }
                    catch (System.Exception)
                    {
                        ErrorMessage = Texts.SERVERERROR;
                    }
                }
                catch (System.Exception e)
                {
                    //TODO
                    //throw;
                }
            }

            IsBusy = false;
        }
    }
}
