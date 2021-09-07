using Microsoft.Xaml.Behaviors;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Moodle.Views {

    public class FocusElementAfterClickBehavior : Behavior<ButtonBase> {
        private ButtonBase _AssociatedButton;

        protected override void OnAttached() {
            _AssociatedButton = AssociatedObject;

            _AssociatedButton.Click += AssociatedButtonClick;
        }

        protected override void OnDetaching() {
            _AssociatedButton.Click -= AssociatedButtonClick;
        }

        void AssociatedButtonClick(object sender, RoutedEventArgs e) {
            Keyboard.Focus(FocusElement);
        }

        public Control FocusElement {
            get { return (Control)GetValue(FocusElementProperty); }
            set { SetValue(FocusElementProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FocusElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusElementProperty =
            DependencyProperty.Register("FocusElement", typeof(Control), typeof(FocusElementAfterClickBehavior), new UIPropertyMetadata());
    }




    public partial class Tab_Student_Subscriptions_View : UserControlBase {

        public Tab_Student_Subscriptions_View() {
            InitializeComponent();
        }

    }

}
