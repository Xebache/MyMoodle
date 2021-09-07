using PRBD_Framework;
using System.Windows;


namespace Moodle.Views {
    
    public partial class Tab_Student_NewCourse_View : UserControlBase {

        public Tab_Student_NewCourse_View() {
            InitializeComponent();
        }

        private void Close_UC(object sender, RoutedEventArgs e) {
            Visibility = Visibility.Hidden;
        }

    }

}
