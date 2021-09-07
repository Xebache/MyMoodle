using Moodle.Models;
using PRBD_Framework;

namespace Moodle.Views {
    
    public partial class Tab_Student_TestCorrection_View : UserControlBase {

        public Tab_Student_TestCorrection_View(Test test) {
            InitializeComponent();
            vm.Init(test);
        }

    }

}
