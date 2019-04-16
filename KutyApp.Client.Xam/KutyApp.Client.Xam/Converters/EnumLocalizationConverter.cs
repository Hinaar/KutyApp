using KutyApp.Client.Xam.Resources.Localization;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Converters
{
    public class EnumLocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            Texts.ResourceManager.GetString(Enum.GetName(value.GetType(), value), Localization.Current.CurrentCultureInfo);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
