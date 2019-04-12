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
	public partial class View1 : ContentView
	{
		public View1 ()
		{
			InitializeComponent ();
		}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            //BindingContext should not be null at this point
            // and you may add your code here.
        }
    }
}