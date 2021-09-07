using Moodle.Models;
using PRBD_Framework;

namespace Moodle.Views {
    
    public partial class Tab_Student_Grade_View : UserControlBase {

        public Tab_Student_Grade_View(Course course) {
            InitializeComponent();
            vm.Init(course);
        }

        private void CustomColumnPosition(object sender, System.EventArgs e) {
            datagrid.Columns[0].DisplayIndex = datagrid.Columns.Count - 1;
        }

    }

}
