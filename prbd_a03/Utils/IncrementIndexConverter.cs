using System;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    class IncrementIndexConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int idx = 0;
            if (value is int)
                idx = (int)value + 1;
            return idx;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
