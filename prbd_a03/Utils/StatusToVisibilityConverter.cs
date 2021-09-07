using Moodle.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Moodle.Utils {
    class StatusToVisibilityConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Subscription.Status status && status == Subscription.Status.Active) {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
