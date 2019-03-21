using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KutyApp.Client.Xam.ContentViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErroredExtraEntry : ContentView
	{
        //TODO: contentview extraentryhez alatta az errosmesaggel, bindablepropertykent errrormessage, sablonba elnevezcni 
        //contentviewt es ugy statickresource path errorsmessagekent beletenni

        public ErroredExtraEntry ()
		{
			InitializeComponent ();
		}
	}
}