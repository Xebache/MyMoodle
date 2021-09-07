using PRBD_Framework;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Moodle.Views {

    public partial class Menu_Teacher_View : UserControlBase {

        public Menu_Teacher_View() {
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
