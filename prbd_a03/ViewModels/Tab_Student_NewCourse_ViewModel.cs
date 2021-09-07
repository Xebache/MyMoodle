using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

	public class Tab_Student_NewCourse_ViewModel : Common_ViewModel {

        #region Fields

        private ObservableCollection<Course> coursesToSubscribeTo;
        public ObservableCollection<Course> CoursesToSubscribeTo { get => coursesToSubscribeTo; set => SetProperty(ref coursesToSubscribeTo, value); }

        public ICommand RegisterCourse { get; set; }
        public ICommand Close { get; set; }

        #endregion

        #region Constructor

        public Tab_Student_NewCourse_ViewModel() : base() {
            OnRefreshData();
            CommandManager();  
        }

        public void InitData() {
            OnRefreshData();
        }

        #endregion

        #region Command

        private void CommandManager() {
            RegisterCourseCommand();
        }

        private void RegisterCourseCommand() {
            RegisterCourse = new RelayCommand<Course>(
                course => NewSubscriptionAction(course), 
                course => CanRegister(course)
            );
        }

        private void AddNewSubscription(Course course) {
            Student student = (Student)LoggedUser;
            Subscription subscription = new(student, course, Subscription.Status.Pending);
            Context.Subscriptions.Add(subscription);
            Context.SaveChanges();
        }

        private void NewSubscriptionAction(Course course) {
            AddNewSubscription(course);
            OnRefreshData();
            NotifyColleagues(AppMessages.MSG_NEWCOURSE_STUDENT);
        }

        private bool CanRegister(Course course) {
            return course != null && !course.IsComplete;
        }
 
        #endregion

        protected override void OnRefreshData() {
            if (IsLoggedUserStudent) {
                Student student = (Student)LoggedUser;
                CoursesToSubscribeTo = new(student.GetCoursesToSubscribeTo());
                RaisePropertyChanged();
            }
            else {
                return;
            }
        }
        
    }

}
