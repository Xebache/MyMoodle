using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    class DateToCalendarMultiConverter : IMultiValueConverter {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0] is DateTime && values[1] is ObservableCollection<DateTime>) {
                DateTime date = (DateTime)values[0];
                ObservableCollection<DateTime> quizDates = values[1] as ObservableCollection<DateTime>;
                return quizDates.Contains(date);
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
