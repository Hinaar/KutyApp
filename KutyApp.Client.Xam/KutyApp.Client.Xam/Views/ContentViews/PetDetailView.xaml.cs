using KutyApp.Client.Xam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KutyApp.Client.Xam.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PetDetailView : ContentView
	{
		public PetDetailView ()
		{
			InitializeComponent ();
		}

        private void OpenImageSelection(object sender, EventArgs e)
        {
            PictureButtonsLayout.IsVisible = !PictureButtonsLayout.IsVisible;
        }

        private async void ShowFullImage(object dender, EventArgs e)
        {
            if (BasicLayout.IsVisible)
            {
                if (this.BindingContext is PetDetailPageViewModel bc)
                    bc.SwipeEnabled = false;

                await BasicLayout.FadeTo(0, 150);
                BasicLayout.IsVisible = false;
                FullImageLayout.IsVisible = true;
                await FullImageLayout.FadeTo(1, 150);
            }
        }

        private async void HideFullImage(object dender, EventArgs e)
        {
            if (this.BindingContext is PetDetailPageViewModel bc)
                bc.SwipeEnabled = true;

            await FullImageLayout.FadeTo(0, 150);
            FullImageLayout.IsVisible = false;
            BasicLayout.IsVisible = true;
            await BasicLayout.FadeTo(1, 150);
        }
    }
}