using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Moodle.Utils {
    class ExpiredDateTimeToVisibilityHiddenConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DateTime input) {
                DateTime today = DateTime.Today;

                if (DateTime.Compare(input, today) < 0)
                    return Visibility.Visible;

                else
                    return Visibility.Hidden;

            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

