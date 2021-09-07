using PRBD_Framework;
using System.Windows.Input;

namespace Moodle.Views {

    public partial class Course_Display_View : UserControlBase {

        public Course_Display_View() {
            InitializeComponent();
        }

        private void ListView_MouseUp(object sender, MouseButtonEventArgs e) {
            vm.DisplayCourse.Execute(listView.SelectedItem);
        }

    }

}
