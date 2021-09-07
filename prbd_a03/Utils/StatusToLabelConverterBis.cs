using Moodle.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Moodle.Utils {

    class StatusToLabelConverterBis : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string label = "";
            if (value is Subscription.Status) {
                Subscription.Status status = (Subscription.Status)value;
                switch (status) {

                    case Subscription.Status.Active:
                        label += "Désinscrire";
                        break;

                    case Subscription.Status.Inactive:
                    case Subscription.Status.Pending:
                        label += "Inscrire";
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
