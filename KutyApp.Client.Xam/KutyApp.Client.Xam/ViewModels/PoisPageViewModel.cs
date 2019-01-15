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
using Plugin.ExternalMaps;

namespace KutyApp.Client.Xam.ViewModels
{
    public class PoisPageViewModel : ViewModelBase
    {
        private IEnvironmentApiService EnvironmentApi { get; }
        private IPageDialogService PageDialogService { get; }


        public PoisPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApiService, IPageDialogService dialogService) : base(navigationService)
        {
            IsBusy = true;
            EnvironmentApi = environmentApiService;
            PageDialogService = dialogService;
            Pois = new ObservableCollection<PoiDto>();
        }

        private ObservableCollection<PoiDto> pois;
        private bool isRefreshing;

        private ICommand openExternalMapCommand;
        private ICommand refreshListCommand;

        public ObservableCollection<PoiDto> Pois
        {
            get { return pois; }
            set { SetProperty(ref pois, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }

        public ICommand OpenExternalMapCommand =>
            openExternalMapCommand ?? (openExternalMapCommand = new Command(
                async poi =>
                   await OpenExternalMapAsync(poi)));

        public ICommand RefreshListCommand =>
           refreshListCommand ?? (refreshListCommand = new Command(
               async () => await LoadPoisAsync(true)));

        private async Task OpenExternalMapAsync(object poi)
        {
           var location = poi as PoiDto;
           await CrossExternalMaps.Current.NavigateTo(location.Name, location.Latitude, location.Longitude);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await Task.Yield();
            await LoadPoisAsync();
            base.OnNavigatedTo(parameters);
        }

        private async Task LoadPoisAsync(bool isRefresh = false)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    //task.yield!
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
                        //CrossLocalNotifications.Current.Show($"{position.Latitude}", $"{position.Longitude}", 101 /*DateTime.Now.AddSeconds(10)*/);

                        bool connected = false;
                        if (CrossConnectivity.IsSupported)
                            connected = CrossConnectivity.Current.IsConnected;

                        if (!connected)
                            await PageDialogService.DisplayAlertAsync("Networking", "Internet connection is down", "OK");

                        if (isRefresh)
                            IsRefreshing = true;
                        else 
                            IsBusy = true;
                        var poik = await EnvironmentApi.GetClosestPoisAsync(new SearchPoiDto {Latitude = position.Latitude, Longitude = position.Longitude });
                        Pois = new ObservableCollection<PoiDto>(poik);

                        if (isRefresh)
                            IsRefreshing = false;
                        else
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
                IsBusy = IsRefreshing = false;
            }
            finally
            {
                IsBusy = IsRefreshing = false;
            }
        }
    }
}
