using Moodle.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {
    public sealed class StatusToLabelConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string label = "";
            if(value is Subscription.Status) {
                Subscription.Status status = (Subscription.Status)value;
                switch (status) {

                    case Subscription.Status.Active:
                    case Subscription.Status.Pending:
                        label += "Se désinscrire";
                        break;

                    case Subscription.Status.Inactive:
                        label += "Se réinscrire";
                        break;
                }
                return label;
            }
            return Binding.DoNothing;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
