using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KutyApp.Client.Xam.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PetDetailPage : ContentPage
	{
		public PetDetailPage ()
		{
			InitializeComponent ();
		}

        private void OpenImageSelection(object sender, SwipedEventArgs e)
        {
            PictureButtonsLayout.IsVisible = !PictureButtonsLayout.IsVisible;
        }

        private async void HideOrShowFullImage(object sender, EventArgs e)
        {
            if (BasicLayout.Opacity != 0)
                await BasicLayout.FadeTo(0, 150);
            else
                await FullImageLayout.FadeTo(0, 150);

            BasicLayout.IsVisible = !BasicLayout.IsVisible;
            FullImageLayout.IsVisible = !FullImageLayout.IsVisible;

            if (BasicLayout.IsVisible)
                await BasicLayout.FadeTo(1, 150);
            else
                await FullImageLayout.FadeTo(1, 150);

        }

    }
}