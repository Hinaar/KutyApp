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
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;

namespace KutyApp.Client.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPetRepository PetRepository { get; }
        private IPageDialogService PageDialogService { get; }

        public MainPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi, IPetRepository petRepository, IPageDialogService dialogService)
            : base(navigationService)
        {
            this.EnvironmentApi = environmentApi;
            this.PetRepository = petRepository;
            this.PageDialogService = dialogService;
            Title = "Main a Page";
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            //TODO: xamarin essentials

            //yield ensures correct working through init methods
            await Task.Yield();

            await GeolocatorMoka();
            base.OnNavigatedTo(parameters);
        }

        public async void GombNyomos()
        {
            await GeolocatorMoka();
        }


        private async Task GeolocatorMoka()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    //Dont call without task.yield!
                    var tmp = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location);

                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await PageDialogService.DisplayAlertAsync("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    Position pos = null;
                    if (CrossGeolocator.IsSupported && CrossGeolocator.Current.IsGeolocationAvailable)
                    {
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 50;
                        var location = await locator.GetLastKnownLocationAsync();
                        
                        //UWP: only this works on desktop and only after allowing location service in location privacy settings
                        var res2 = await locator.GetPositionAsync();
                        string s = string.Empty;
                        await PageDialogService.DisplayAlertAsync("Ittvagy", $"{res2.Latitude}   -    {res2.Longitude}", "OK");
                    }

                    var results = await CrossGeolocator.Current.GetPositionAsync();
                    string ss = string.Empty;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await PageDialogService.DisplayAlertAsync("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task ConnectivityMoka()
        {
            bool connected = false;
            if (CrossConnectivity.IsSupported)
                connected = CrossConnectivity.Current.IsConnected;

            var tmp = await PetRepository.GetDogsAsync();
            //var netTmp = await EnvironmentApi.GetPoisAsync();
            Title = tmp.FirstOrDefault()?.Name ?? "ures";
        }
    }
}
