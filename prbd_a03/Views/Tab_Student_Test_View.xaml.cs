using Moodle.Models;
using PRBD_Framework;
using System.Windows;
using System.Windows.Input;

namespace Moodle.Views {

    public partial class Tab_Student_Test_View : UserControlBase {

        public Tab_Student_Test_View(Test test) {
            InitializeComponent();
            vm.Init(test);
        }

        private void SelectedItem_MouseUp(object sender, MouseButtonEventArgs e) {
            vm.TestQuestionSelected.Execute(lvTest.SelectedItem);
        }

        private void Checkbox_Checked(object sender, RoutedEventArgs e) {
            vm.ChoicesAdded.Execute(lvQuestions.SelectedItems);
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e) {
            vm.ChoicesAdded.Execute(lvQuestions.SelectedItems);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e) {
            vm.ChoicesAdded.Execute(lvQuestions.SelectedItems);
        }
    }
}
