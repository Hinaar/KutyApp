﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace KutyApp.Client.Xam.Converters
{
    public class IsBindingNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value != null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
