using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using KutyApp.Client.Services.LocalRepository.Interfaces;
using KutyApp.Client.Services.LocalRepository.Managers;
using Prism;
using Prism.Ioc;
using System.Diagnostics;
using System.IO;

namespace KutyApp.Client.Xam.Droid
{
    [Activity(Label = "KutyApp.Client.Xam", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "KutyAppDb.db");

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer(dbPath)));
        }
    }



    public class AndroidInitializer : IPlatformInitializer
    {
        private string path;
        public AndroidInitializer(string dbPath)
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

