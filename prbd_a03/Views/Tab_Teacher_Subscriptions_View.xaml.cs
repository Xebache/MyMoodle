using Moodle.Models;
using PRBD_Framework;


namespace Moodle.Views {
    
    public partial class Tab_Teacher_Subscriptions_View : UserControlBase {

        public Tab_Teacher_Subscriptions_View(Course course) {
            InitializeComponent();
            vm.Init(course);
        }

        
    }
}
