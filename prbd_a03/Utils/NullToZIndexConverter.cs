using Moodle.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    class NullToZIndexConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is ObservableCollection<QuizQuestion> selected && selected.Count == 0) {
                return 1;
            }
            return 0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
