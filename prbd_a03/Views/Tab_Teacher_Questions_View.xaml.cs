using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Moodle.Views {
    
    public partial class Tab_Teacher_Questions_View : UserControlBase {

        public Tab_Teacher_Questions_View(Course course) {
            InitializeComponent();
            vm.Init(course);
        }

    }

}
