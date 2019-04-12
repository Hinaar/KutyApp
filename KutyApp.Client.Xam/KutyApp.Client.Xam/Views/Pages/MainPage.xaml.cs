using KutyApp.Client.Xam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Behaviors;

namespace KutyApp.Client.Xam.Views
{
    public partial class MainPage : ContentPage
    {
        private TempPage loginpage;
        public MainPage()
        {
            InitializeComponent();
            loginpage = new TempPage();
        }
    }
}