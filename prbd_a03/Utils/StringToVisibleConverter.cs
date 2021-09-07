using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Moodle.Utils {
    class StringToVisibleConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value != null && value is string) {
                return value.Equals("Total") || value.Equals("Moyenne") ? Visibility.Hidden : Visibility.Visible;
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
