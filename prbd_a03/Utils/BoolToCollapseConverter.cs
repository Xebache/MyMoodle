using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Moodle.Utils {

    [ValueConversion(typeof(object), typeof(Visibility))]

    class BoolToCollapseConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is bool) {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
            else {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
