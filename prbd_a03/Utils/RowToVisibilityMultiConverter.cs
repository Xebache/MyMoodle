using Moodle.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Moodle.Utils {
    class RowToVisibilityMultiConverter : IMultiValueConverter {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values[0] is string title && values[1] is Course course) {
                Quiz quiz = course.Quiz.Where(q => q.Title == title).FirstOrDefault();
                if (quiz != null) {
                    DateTime current = DateTime.Now;
                    return quiz.EndAt.Date >= DateTime.Now.Date ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
