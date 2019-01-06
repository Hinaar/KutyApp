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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PictureButtonsLayout.IsVisible = !PictureButtonsLayout.IsVisible;
        }
    }
}