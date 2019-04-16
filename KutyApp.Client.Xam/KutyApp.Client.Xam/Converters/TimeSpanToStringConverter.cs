using System;
using System.Globalization;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            DateTime dateTime = DateTime.MinValue + timeSpan;

            // acknowledgement: based on some code from 
            // http://stackoverflow.com/questions/1292246/why-doesnt-datetime-toshorttimestring-respect-the-short-time-format-in-regio

            DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            string shortTimePattern
                = dateTimeFormat.LongTimePattern.Replace(":ss", String.Empty).Replace(":s", String.Empty);
            return dateTime.ToString(shortTimePattern);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("TimeSpanToTimeStringConverter.ConvertBack");
        }
    }
}
