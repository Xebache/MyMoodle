using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Moodle.Utils {

    class ClearOnFocusedBehavior : Behavior<Control> {

        private readonly RoutedEventHandler _onGotFocusHandler = (o, e) => {
            if (o is TextBox) {
                ((TextBox)o).Text = string.Empty;
            }
            if (o is PasswordBox) {
                ((PasswordBox)o).Password = string.Empty;
            }
        };

        protected override void OnAttached() {
            AssociatedObject.GotFocus += _onGotFocusHandler; 
        }

        protected override void OnDetaching() {
            AssociatedObject.GotFocus -= _onGotFocusHandler;
        }

    }

}
