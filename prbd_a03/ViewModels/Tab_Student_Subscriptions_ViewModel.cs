using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

	public class Tab_Student_Subscriptions_ViewModel : Common_ViewModel {

        #region Fields

        private ObservableCollection<Subscription> _subscriptions = new();
        public ObservableCollection<Subscription> Subscriptions { get => _subscriptions; set => SetProperty(ref _subscriptions, value); }

        public ICommand Resubscribe { get; set; }

        #endregion

        #region Constructor

        public Tab_Student_Subscriptions_ViewModel() : base() {
            OnRefreshData();
            CommandsManager();
        }

		#endregion

		#region Commands

        private void CommandsManager() {
            ResubscribeCommand();
            NewCourseStudent_Register();
            NewCourseTeacher_Register();
            StatusChanged_Register();
        }

		private void ResubscribeCommand() {
            Resubscribe = new RelayCommand<Subscription>(subscription => {
                if (subscription.SubscriptionStatus.Equals(Subscription.Status.Inactive)) {
                    subscription.SubscriptionStatus = Subscription.Status.Pending;
                }
                else {
                    subscription.SubscriptionStatus = Subscription.Status.Inactive; 
                }
                Context.Subscriptions.Update(subscription);
                Context.SaveChanges();
                RaisePropertyChanged();

            });
        }

        private void NewCourseStudent_Register() {
            Register(this, AppMessages.MSG_NEWCOURSE_STUDENT, OnRefreshData);
        }

        private void NewCourseTeacher_Register() {
            Register(this, AppMessages.MSG_NEWCOURSE_TEACHER, OnRefreshData);
        }

        private void StatusChanged_Register() {
            Register(this, AppMessages.MSG_STATUS_CHANGED,() => OnRefreshData());
        }

        #endregion

        protected override void OnRefreshData() {
            if (IsLoggedUserStudent) {
                Student student = (Student)LoggedUser;
                _subscriptions = new ObservableCollection<Subscription>(student.GetSubscriptions());
                RaisePropertyChanged();
            }
            else {
                return;
            }
        }
    }
}
