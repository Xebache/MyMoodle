using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

	public class Menu_Teacher_ViewModel : Common_ViewModel {

        #region Fields

        private ObservableCollection<Course> _courses = new();
        public ObservableCollection<Course> Courses { get => _courses; set => SetProperty(ref _courses, value, OnRefreshData); }

        public ICommand QuestionsTeacher { get; set; }
        public ICommand QuizTeacher { get; set; }
        public ICommand GradesTeacher { get; set; }
        public ICommand SubscriptionsTeacher { get; set; }
        public ICommand CourseDetails { get; set; }
        
        #endregion

        #region Constructor

        public Menu_Teacher_ViewModel() : base() {
            InitData();
            CommandsManager();
            
        }

        private void InitData() {
            if(IsLoggedUserTeacher) {
                Teacher teacher = (Teacher)LoggedUser;
                _courses = teacher.GetCourses();
            }
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            QuestionsTeacherCommand();
            QuizTeacherCommand();
            GradesTeacherCommand();
            SubscriptionsTeacherCommand();
            StatusChanged_Register();
            NewCourseTeacher_Register();
            RefreshCourse_Register();
            CourseDetailsCommand();
        }

        private void CourseDetailsCommand() {
            CourseDetails = new RelayCommand(() => 
                NotifyColleagues(AppMessages.VIEW_COURSE_DETAILS_TEACHER)
            );
        }

        private void QuestionsTeacherCommand() {
            QuestionsTeacher = new RelayCommand<Course>(course => 
                NotifyColleagues(AppMessages.VIEW_QUESTIONS_TEACHER, course)
            );
        }

        private void QuizTeacherCommand() {
            QuizTeacher = new RelayCommand<Course>(course => 
                NotifyColleagues(AppMessages.VIEW_QUIZ_TEACHER, course)
            );
        }

        private void GradesTeacherCommand() {
            GradesTeacher = new RelayCommand<Course>(course => 
                NotifyColleagues(AppMessages.VIEW_GRADES_TEACHER, course)
            );
        }

        private void SubscriptionsTeacherCommand() {
            SubscriptionsTeacher = new RelayCommand<Course>(course => 
                NotifyColleagues(AppMessages.VIEW_SUBSCRIPTIONS_TEACHER, course)
            );
        }

        private void StatusChanged_Register() {
            Register(this, AppMessages.MSG_STATUS_CHANGED, () => {
                Teacher teacher = (Teacher)LoggedUser;
                _courses = teacher.GetCourses();
                RaisePropertyChanged();
            });
        }

        private void NewCourseTeacher_Register() {
            Register(this, AppMessages.MSG_NEWCOURSE_TEACHER, OnRefreshData);
        }

        private void RefreshCourse_Register() {
            Register(this, AppMessages.MSG_UPDATECOURSES_TEACHER, OnRefreshData);
        } 

        #endregion

        protected override void OnRefreshData() {
            if (IsLoggedUserTeacher) {
                Teacher teacher = (Teacher)LoggedUser;
                _courses = teacher.GetCourses();
            }
            RaisePropertyChanged();
        }

    }

}
