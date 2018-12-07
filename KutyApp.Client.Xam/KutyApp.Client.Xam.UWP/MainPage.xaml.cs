using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.LocalRepository.Managers;
using Prism;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace KutyApp.Client.Xam.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, Common.Constants.Paths.DbName);

            LoadApplication(new KutyApp.Client.Xam.App(new UwpInitializer(dbPath)));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        private string path;
        public UwpInitializer(string dbPath)
        {
            path = dbPath;
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterInstance<IPetRepository>(new PetRepositoryManager(path));
        }
    }
}
