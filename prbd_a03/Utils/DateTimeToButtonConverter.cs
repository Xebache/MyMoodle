using System;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    class DateTimeToButtonConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DateTime input) {
                DateTime today = DateTime.Today;

                if (DateTime.Compare(input, today) >= 0)
                    return "Passer le test";
                else
                    return "Voir les résultats";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
