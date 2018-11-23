using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Xam.Config;
using KutyApp.Client.Xam.ViewModels;
using KutyApp.Client.Xam.Views;
using Prism;
using Prism.Ioc;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KutyApp.Client.Xam
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
            //await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(RestService.For<IEnvironmentApiService>(Configurations.ConnectionBase));
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<PetsPage, PetsPageViewModel>();
            //containerRegistry.RegisterInstance<IPetRepository>(new PetRepositoryManager(localDbPath));
        }
    }
}
