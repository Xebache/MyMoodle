using Moodle.Authentication;
using Moodle.Models;
using Moodle.Utils;
using PRBD_Framework;
using System.Linq;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class UpdateUser_ViewModel : Common_ViewModel {

        #region Fields

        private User _user = LoggedUser;
        public User User { get => _user; set => SetProperty(ref _user, value); }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _fullName;
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _confirm;
        public string Confirm { get => _confirm; set => SetProperty(ref _confirm, value); }

        private string _error;
        public string Error { get => _error; set => SetProperty(ref _error, value); }

        public ICommand SaveUpdate { get; set; }
        public ICommand CancelUpdate { get; set; }

        #endregion

        #region Constructor

        public UpdateUser_ViewModel() : base() {
            InitData();
            CommandsManager();
        }

        private void InitData() {
            _email = _user.Email;
            _fullName = _user.FullName;
            _password = _user.Password;
            _confirm = "";
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            SaveUpdateCommand();
            CancelUpdateCommand();
        }

        public void SaveUpdateCommand() {
            SaveUpdate = new RelayCommand(UpdateUserAction, CanUpdate);
        }

        private void CancelUpdateCommand() {
            CancelUpdate = new RelayCommand(OnRefreshData);
        }

        #endregion

        #region Validation

        private void UpdateUserAction() {
            if (Validate()) {
                var user = UpdateUser();
                Login(user);
                Context.SaveChanges();
                RaisePropertyChanged();
                OnRefreshData();
                NotifyColleagues(AppMessages.MSG_LOGGED_UPDATED);
            }
        }

        private bool CanUpdate() {
            return (Email != User.Email || FullName != User.FullName || Password != User.Password)
                && Email != "" && FullName != "" && Password != "";
        }

        public override bool Validate() {
            ClearErrors();

            User user = NewUser();
            if (Validator.EmailAlreadyTaken(Email)) {
                AddError(nameof(Error), "Email déjà associé à un identifiant");
            }
            else if (!Validator.IsEmailFormatValid(Email)) {
                AddError(nameof(Error), "Format du mail invalide");
            }
            else if (!Validator.IsFullNameValid(FullName)) {
                AddError(nameof(Error), "Format du pseudo invalide");
            }
            else if (!Validator.IsPasswordFormatValid(Password)) {
                AddError(nameof(Error), "Format de mot de passe invalide");
            }
            else if (!Validator.ArePasswordsMatching(Password, Confirm)) {
                AddError(nameof(Error), "Les mots de passe ne correspondent pas");
            }

            RaiseErrors();
            return !HasErrors;
        }

        private User NewUser() {
            User user;
            if (IsLoggedUserStudent) {
                user = new Student(FullName, Email, Password);
            }
            else {
                user = new Teacher(FullName, Email, Password);
            }
            return user;
        }

        private User UpdateUser() {
            var user = Context.Users.Single(user => user == LoggedUser);
            user.Email = Email;
            user.FullName = FullName;
            user.Password = Password;
            Context.SaveChanges();
            return user;
        }

        #endregion

        protected override void OnRefreshData() {
            InitData();
            RaisePropertyChanged();
        }

    }

}
