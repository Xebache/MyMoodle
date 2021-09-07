using Moodle.Models;
using PRBD_Framework;
using System.Windows;

namespace Moodle.Views {
    
    public partial class Questions_Edit_View : UserControlBase {

        public static readonly DependencyProperty CourseProperty =
            DependencyProperty.Register("Course", typeof(Course), typeof(Questions_Edit_View), new PropertyMetadata(null));

        public Course Course {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }

        public Questions_Edit_View() {
            InitializeComponent();
        }

    }

}
