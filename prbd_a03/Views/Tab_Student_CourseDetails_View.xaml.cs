using Moodle.Models;
using PRBD_Framework;

namespace Moodle.Views {

    public partial class Tab_Student_CourseDetails_View : UserControlBase {

        public Tab_Student_CourseDetails_View(Course course) {
            InitializeComponent();
            vm.Init(course);
        }

    }

}
