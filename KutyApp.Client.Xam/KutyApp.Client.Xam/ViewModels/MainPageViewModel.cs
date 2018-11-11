using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using KutyApp.Client.Services.ClientConsumer.Interfaces;
using KutyApp.Client.Services.LocalRepository.Interfaces;

namespace KutyApp.Client.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPetRepository PetRepository { get; }
        public MainPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IPetRepository petRepository)
            : base(navigationService)
        {
            this.EnvironmentApi = environmentApi;
            this.PetRepository = petRepository;
            Title = "Main a Page";
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var tmp = await PetRepository.GetDogsAsync();
            //var tmp = await EnvironmentApi.GetPoisAsync();
            Title = tmp.FirstOrDefault()?.Name ?? "ures";
            base.OnNavigatedTo(parameters);
        }
    }
}
