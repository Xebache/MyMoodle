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
    
    public partial class Menu_Student_View : UserControlBase {

        public Menu_Student_View() {
            InitializeComponent();
        }

        //ContextMenu
        private void ShowContextMenu_Click(object sender, RoutedEventArgs e) {
            if (sender is FrameworkElement addButton) {
                addButton.ContextMenu.IsOpen = true;
            }
        }

    }

}
