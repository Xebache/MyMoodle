using Moodle.Models;
using PRBD_Framework;

namespace Moodle.Views {
    
    public partial class Tab_Student_TestList_View : UserControlBase {

        public Tab_Student_TestList_View(Course Course) {
            InitializeComponent();
            vm.Init(Course);
        }

    }

}
