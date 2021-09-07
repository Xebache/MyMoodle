﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Moodle.Utils {
    [ValueConversion(typeof(object), typeof(Visibility))]

    public class BoolToVisibleHiddenConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value != null && value is bool) {
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
            }
            else {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

    }
}
