using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Moodle.Views {

    public partial class Questions_List_View : UserControlBase {

        public Questions_List_View() {
            InitializeComponent();
        }

        public Course Course {
            get { return (Course)GetValue(CourseProperty); }
            set { SetValue(CourseProperty, value); }
        }

        public static readonly DependencyProperty CourseProperty =
            DependencyProperty.Register("Course", typeof(Course), typeof(Questions_List_View), new PropertyMetadata(null));

        public ObservableCollection<Question> Questions {
            get { return (ObservableCollection<Question>)GetValue(QuestionsProperty); }
            set { SetValue(QuestionsProperty, value); }
        }

        public static readonly DependencyProperty QuestionsProperty =
            DependencyProperty.Register("Questions", typeof(ObservableCollection<Question>), typeof(Questions_List_View), new PropertyMetadata(null));


        public ObservableCollection<Category> Categories {
            get { return (ObservableCollection<Category>)GetValue(CategoriesProperty); }
            set { SetValue(QuestionsProperty, value); }
        }

        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(ObservableCollection<Category>), typeof(Questions_List_View), new PropertyMetadata(null));
   
    }

}
