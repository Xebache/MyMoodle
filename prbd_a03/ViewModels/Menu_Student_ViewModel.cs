using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {
    public class Menu_Student_ViewModel : Common_ViewModel {

        #region Fields

        private ObservableCollection<Subscription> _subscriptions = new ObservableCollection<Subscription>();
        public ObservableCollection<Subscription> Subscriptions { get => _subscriptions; set => SetProperty(ref _subscriptions, value); }

        public ICommand NewCourseStudent { get; set; }
        public ICommand QuizStudent { get; set; }
        public ICommand GradeStudent { get; set; }
        public ICommand SubscriptionsStudent { get; set; }
        public ICommand HomeStudent { get; set; }

        #endregion

        #region Constructor

        public Menu_Student_ViewModel() : base() {
            OnRefreshData();
            CommandsManager();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            NewCourseStudent_Command();
            QuizStudent_Command();
            GradeStudent_Command();
            SubscriptionsStudent_Command();
            NewCourseStudent_Register();
            StatusChanged_Register();
            HomeStudentCommand();
        }

        private void NewCourseStudent_Command() {
            NewCourseStudent = new RelayCommand(() =>
                NotifyColleagues(AppMessages.VIEW_NEW_COURSE_STUDENT)
            );
        }

        private void QuizStudent_Command() {
            QuizStudent = new RelayCommand<Subscription>(subscription => {
                Course course = subscription.Course;
                NotifyColleagues(AppMessages.VIEW_TESTLIST_STUDENT, course);
                }
            );
        }

        private void GradeStudent_Command() {
            GradeStudent = new RelayCommand<Subscription>(subscription => {
                Course course = subscription.Course;
                NotifyColleagues(AppMessages.VIEW_GRADE_STUDENT, course);
            });
        }

        private void SubscriptionsStudent_Command() {
            SubscriptionsStudent = new RelayCommand(() => 
                NotifyColleagues(AppMessages.VIEW_SUBSCRIPTIONS_STUDENT)
            );
        }

        private void HomeStudentCommand() {
            HomeStudent = new RelayCommand<Subscription>(subscription => {
                Course course = subscription.Course;
                NotifyColleagues(AppMessages.VIEW_COURSE_DETAILS_STUDENT, course);
            });
        }

        private void NewCourseStudent_Register() {
            Register(this, AppMessages.MSG_NEWCOURSE_STUDENT, OnRefreshData);
        }

        private void StatusChanged_Register() {
            Register(this, AppMessages.MSG_STATUS_CHANGED, () => OnRefreshData());
        }

        #endregion

        protected override void OnRefreshData() {
            if (IsLoggedUserStudent) {
                Student student = (Student)LoggedUser;
                _subscriptions = student.GetSubscriptions();
            }
            RaisePropertyChanged();
        }

    }

}
