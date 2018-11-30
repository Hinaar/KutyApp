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
using Plugin.LocalNotifications;
using System.Windows.Input;
using Xamarin.Forms;
using KutyApp.Client.Services.LocalRepository.Entities.Models;
using System.Collections.ObjectModel;
using KutyApp.Client.Services.ClientConsumer.Dtos;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PoisPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPageDialogService PageDialogService { get; }


        public PoisPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApiService, IPageDialogService dialogService) : base(navigationService)
        {
            EnvironmentApi = environmentApiService;
            PageDialogService = dialogService;
            Pois = new ObservableCollection<PoiDto>();
        }

        private ObservableCollection<PoiDto> pois;

        public ObservableCollection<PoiDto> Pois
        {
            get { return pois; }
            set { SetProperty(ref pois, value); }
        }

        private ObservableCollection<Dog> dogs;

        public ObservableCollection<Dog> Dogs
        {
            get { return dogs; }
            set { SetProperty(ref dogs, value); }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await Task.Yield();
            await LoadPois();
        }

        private async Task LoadPois()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    //Dont call without task.yield!
                    var tmp = await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location);

                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                        await PageDialogService.DisplayAlertAsync("Need location", "Gunna need that location", "OK");

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                   
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    if (CrossGeolocator.IsSupported && CrossGeolocator.Current.IsGeolocationAvailable)
                    {
                        var locator = CrossGeolocator.Current;
                        locator.DesiredAccuracy = 50;
                        //var location = await locator.GetLastKnownLocationAsync();

                        //UWP: only this works on desktop and only after allowing location service in location privacy settings
                        var position = await locator.GetPositionAsync();
                        CrossLocalNotifications.Current.Show($"{position.Latitude}", $"{position.Longitude}", 101 /*DateTime.Now.AddSeconds(10)*/);

                        bool connected = false;
                        if (CrossConnectivity.IsSupported)
                            connected = CrossConnectivity.Current.IsConnected;

                        if (!connected)
                            await PageDialogService.DisplayAlertAsync("Networking", "Internet connection is down", "OK");

                        IsBusy = true;
                        var poik = await EnvironmentApi.GetClosestPoisAsync(new SearchPoiDto {Latitude = position.Latitude, Longitude = position.Longitude });
                        Pois = new ObservableCollection<PoiDto>(poik);
                        IsBusy = false;
                    }
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
    }
}
