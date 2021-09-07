using System;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    class CategoryToBool : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0] != null)
                return true;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
