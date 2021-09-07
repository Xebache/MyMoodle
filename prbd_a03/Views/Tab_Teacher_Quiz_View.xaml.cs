using PRBD_Framework;
using Moodle.Models;
using System.Windows.Input;
using System.Windows;

namespace Moodle.Views {

    public partial class Tab_Teacher_Quiz_View : UserControlBase {

        public Tab_Teacher_Quiz_View(Course course) {
            InitializeComponent();
            vm.Init(course);
        }

        private void SelectedItem_MouseUp(object sender, MouseButtonEventArgs e) {
            vm.QuizQuestionSelected.Execute(lvQuizQuestions.SelectedItem);
        }
        
    }

}
