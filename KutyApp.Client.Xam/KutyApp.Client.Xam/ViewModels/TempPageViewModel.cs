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
using KutyApp.Client.Common.Constants;
using System.Diagnostics;
using KutyApp.Client.Xam.Views;

namespace KutyApp.Client.Xam.ViewModels
{
    public class TempPageViewModel : ViewModelBase
    {
        public TempPageViewModel(INavigationService navigationService, IEnvironmentApiService environmentApi,IPageDialogService dialogService)
            : base(navigationService)
        {
            Title = "Viewmodel title";
        }

    }
}
