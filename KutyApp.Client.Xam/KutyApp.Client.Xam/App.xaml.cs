﻿using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.ServiceCollector.Implementations;
using KutyApp.Client.Services.ServiceCollector.Interfaces;
using KutyApp.Client.Xam.Config;
using KutyApp.Client.Xam.Resources.Localization;
using KutyApp.Client.Xam.ViewModels;
using KutyApp.Client.Xam.Views;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Refit;
using System.Threading.Tasks;
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
            //TODO:
            Localization.Current.OnCultureChanged += (culture) => Texts.Culture = culture;

            Localization.Current.EnsureDeviceOrDefaultCulture(defaultCultureName: "en",
                                    availableCultures: new[] { "en", "hu" });

            InitializeComponent();

            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
            await NavigationService.NavigateAsync(nameof(Views.LoadingPage));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterSingleton<IKutyAppClientContext, KutyAppClientContext>();
            containerRegistry.RegisterInstance(RestService.For<IEnvironmentApiService>(Configurations.ConnectionBase, new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => GetApiKey() 
            }));

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoadingPage, LoadingPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<PetsPage, PetsPageViewModel>();
            containerRegistry.RegisterForNavigation<PetDetailPage, PetDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<PoisPage, PoisPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<AdvertsPage, AdvertsPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPopupPage>();
            containerRegistry.RegisterForNavigation<RegisterPopupPage>();
            containerRegistry.RegisterForNavigation<PetHabitPopupPage>();
            containerRegistry.RegisterForNavigation<PetMedicalTreatmentPopupPage>();
            containerRegistry.RegisterForNavigation<PetSitterPopupPage>();
            containerRegistry.RegisterForNavigation<AdvertisementPopupPage>();
            containerRegistry.Register<IPermissionManager, PermissionManager>();
            containerRegistry.Register<IMediaManager, MediaManager>();
            //containerRegistry.RegisterInstance<IPetRepository>(new PetRepositoryManager(localDbPath));
        }

        private async Task<string> GetApiKey() => await Container.Resolve<IKutyAppClientContext>().ReturnApiKeyAsync();
    }
}
