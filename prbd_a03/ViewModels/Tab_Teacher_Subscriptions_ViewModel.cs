using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Tab_Teacher_Subscriptions_ViewModel : Common_ViewModel {

        #region Fields

        private Course _course;
        public Course Course { 
            get => _course; 
            set => SetProperty(ref _course, value); 
        }

        private ObservableCollection<Subscription> _subscriptions;
        public ObservableCollection<Subscription> Subscriptions {
            get => _subscriptions;
            set => SetProperty(ref _subscriptions, value);
        }

        private ObservableCollection<User> _studentsToAdd;
        public ObservableCollection<User> StudentsToAdd {
            get => _studentsToAdd;
            set => SetProperty(ref _studentsToAdd, value);
        }

        private bool _activeSelected;
		public bool ActiveSelected {
			get => _activeSelected;
            set => SetProperty(ref _activeSelected, value, OnRefreshData);
        }

        private bool _pendingSelected;
		public bool PendingSelected {
			get => _pendingSelected;
			set => SetProperty(ref _pendingSelected, value, OnRefreshData); 
        }

        private bool _inactiveSelected;
		public bool InactiveSelected {
			get => _inactiveSelected;
			set => SetProperty(ref _inactiveSelected, value, OnRefreshData);
		}

        private bool _allSelected;
		public bool AllSelected {
			get => _allSelected;
			set => SetProperty(ref _allSelected, value, OnRefreshData); 
        }

        public ICommand RegisterUnregister { get; set; }
        public ICommand AddNewStudent { get; set; }

        #endregion

        #region Constructor

        public Tab_Teacher_Subscriptions_ViewModel() : base() {
            CommandsManager();
        }

        public void Init(Course course) {
            Course = course;
            AllSelected = true;
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            RegisterUnregisterCommand();
            AddNewStudentCommand();
        }

        private void RegisterUnregisterCommand() {
            RegisterUnregister = new RelayCommand<Subscription>(subscription => {
                if (subscription.SubscriptionStatus.Equals(Subscription.Status.Active)) {
                    subscription.SubscriptionStatus = Subscription.Status.Inactive;
                }
                else {
                    subscription.SubscriptionStatus = Subscription.Status.Active;
                }
                Context.Subscriptions.Update(subscription);
                Context.SaveChanges();
                OnRefreshData();
                RaisePropertyChanged();
                NotifyColleagues(AppMessages.MSG_STATUS_CHANGED);
            });
        }

        private void AddNewStudentCommand() {
            AddNewStudent = new RelayCommand<User>(
                user => AddNewSubscriptionAction(user), 
                user => user != null && !Course.IsComplete
            );
        }

        private void StatusChangedNotification() {
            OnRefreshData();
            RaisePropertyChanged();
            NotifyColleagues(AppMessages.MSG_STATUS_CHANGED);
        }

        private void AddNewSubscriptionAction(User user) {
            Subscription subscription = new ((Student)user, Course, Subscription.Status.Active);
            Context.Subscriptions.Add(subscription);
            Context.SaveChanges();
            StatusChangedNotification();
        }
 
        #endregion

        #region Filter

        private Subscription.Status BoolToStatus() {
            if (_activeSelected)
                return Subscription.Status.Active;
            else if (_pendingSelected)
                return Subscription.Status.Pending;
            else 
                return Subscription.Status.Inactive;
        }

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            Subscriptions = _allSelected ? 
                Course.GetAllSubscriptions() 
                : 
                Course.GetFilteredSubscriptions(BoolToStatus());
            StudentsToAdd = Course.GetStudentsToAdd();
            RaisePropertyChanged();
        }

        #endregion

    }

}
