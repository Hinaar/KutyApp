using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace KutyApp.Client.Common.Constants
{
    public static class Languages
    {
        public static CultureInfo Default = Localization.Current.GetCultureInfo("en");
        public static CultureInfo En = Localization.Current.GetCultureInfo("en");
        public static CultureInfo Hu = Localization.Current.GetCultureInfo("hu");
    }
}
