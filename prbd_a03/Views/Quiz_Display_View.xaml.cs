using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class Quiz_Display_View : UserControlBase {

        public Course Course {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }

        public static readonly DependencyProperty CourseProperty =
            DependencyProperty.Register("Course", typeof(Course), typeof(Quiz_Display_View), new PropertyMetadata(null));

        public ObservableCollection<QuizQuestion> QuizQuestions {
            get { return (ObservableCollection<QuizQuestion>)GetValue(QuizQuestionsProperty); }
            set { SetValue(QuizQuestionsProperty, value); }
        }

        public static readonly DependencyProperty QuizQuestionsProperty =
            DependencyProperty.Register("QuizQuestions", typeof(ObservableCollection<QuizQuestion>), typeof(Quiz_Display_View), new PropertyMetadata(null));

        public Quiz_Display_View() {
            InitializeComponent();
        }

        private void SelectedItem_MouseUp(object sender, MouseButtonEventArgs e) {
            vm.QuizQuestionSelected.Execute(lvQuizQuestions.SelectedItem);
        }


        

    }

}
