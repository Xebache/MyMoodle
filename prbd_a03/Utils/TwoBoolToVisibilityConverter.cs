using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Moodle.Utils {

    class TwoBoolToVisibilityConverter : IMultiValueConverter {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0] is bool correct && values[1] is bool selected) {
                if ((correct && !selected) || (!correct && selected)) {
                    return Visibility.Visible;
                }
            }
            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
