using KutyApp.Client.Services.ServiceCollector.Interfaces;
using Prism.Navigation;

namespace KutyApp.Client.Xam.ViewModels
{
    public class LoadingPageViewModel : ViewModelBase
    {
        private IKutyAppClientContext KutyAppClientContext { get; }
        public LoadingPageViewModel(INavigationService navigationService, IKutyAppClientContext kutyAppClientContext)
            : base(navigationService)
        {
            KutyAppClientContext = kutyAppClientContext;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await KutyAppClientContext.LoadSettingsAsync();
            CurrentLanguage = KutyAppClientContext.SavedLanguage;

            await NavigationService.NavigateAsync("app:///MainPage", animated: false);
        }
    }
}
