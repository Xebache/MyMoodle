using System;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    class DateTimeToCommandConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DateTime input) {
                DateTime today = DateTime.Today;

                if (DateTime.Compare(input, today) >= 0)
                    return "GoToTest";
                else
                    return "GoToGrades";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

    }
}
