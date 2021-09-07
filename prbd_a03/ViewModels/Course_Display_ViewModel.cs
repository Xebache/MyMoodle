using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

    class Course_Display_ViewModel : Common_ViewModel {

        #region Fields

        private ObservableCollection<Course> _courses;
        public ObservableCollection<Course> Courses {
            get => _courses;
            set => SetProperty(ref _courses, value);
        }

        private Course _course;
        public Course Course {
            get => _course;
            set => SetProperty(ref _course, value, OnRefreshData);
        }

        public ICommand NewCourse { get; set; }
        public ICommand DisplayCourse { get; set; }

        #endregion

        #region Constructor

        public Course_Display_ViewModel() {
            OnRefreshData();
            CommandsManager();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            NewCourseCommand();
            DisplayCourseCommand();
            RefreshCourseRegister();
            StatusChanged_Register();
        }

        private void NewCourseCommand() {
            NewCourse = new RelayCommand(() => NotifyColleagues(AppMessages.MSG_CREATE_NEW_COURSE));
        }

        private void DisplayCourseCommand() {
            DisplayCourse = new RelayCommand<Course>(course => NotifyColleagues(AppMessages.MSG_COURSE_TO_DISPLAY, course));
        }

        private void RefreshCourseRegister() {
            Register(this, AppMessages.MSG_UPDATECOURSES_TEACHER, OnRefreshData);
        }

        private void StatusChanged_Register() {
            Register(this, AppMessages.MSG_STATUS_CHANGED, OnRefreshData);
        }

        #endregion

        protected override void OnRefreshData() {
            if (IsLoggedUserTeacher) {
                Teacher teacher = (Teacher)LoggedUser;
                _courses = teacher.GetCourses();
                RaisePropertyChanged();
            }

            
        }

    }

}
