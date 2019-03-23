using System;
using System.Globalization;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Converters
{
    public class IsTextEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            string.IsNullOrEmpty((value ?? string.Empty) as string);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
