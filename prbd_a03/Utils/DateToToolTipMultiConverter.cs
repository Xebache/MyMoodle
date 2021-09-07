using Moodle.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Moodle.Utils {

    class DateToToolTipMultiConverter : IMultiValueConverter {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0] is DateTime date && values[1] is ObservableCollection<DateTime> && values[2] is Course course) {
                ObservableCollection<DateTime> quizDates = values[1] as ObservableCollection<DateTime>;
                if (quizDates.Contains(date)) {
                    return course.Quiz.Where(q => q.StartAt <= date && q.EndAt >= date).Select(q => q.Title).FirstOrDefault();
                }
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
