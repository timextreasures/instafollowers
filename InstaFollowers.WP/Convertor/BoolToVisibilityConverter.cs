﻿using System;
using System.Windows;
using System.Windows.Data;

namespace InstaFollowers.WP.Convertor
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "reverse")
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
