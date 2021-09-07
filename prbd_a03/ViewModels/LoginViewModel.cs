using Moodle.Authentication;
using Moodle.Models;
using Moodle.Utils;
using PRBD_Framework;
using System;
using System.Windows;
using System.Windows.Input;

namespace Moodle.ViewModels {

	public class LoginViewModel : Common_ViewModel {

        #region Fields

        private string _email = "Email";
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _password = "********";
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _error;
        public string Error { get => _error; set => SetProperty(ref _error, value); }

        public event Action OnLoginSuccess;

        public ICommand LogIn { get; set; }
        public ICommand Quit { get; set; }

        public ICommand LoginStudent { get; set; }
        public ICommand LoginTeacher { get; set; }

        #endregion

        #region Constructor

        public LoginViewModel() : base() {
            CommandsManager();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            LoginCommand();
            QuitCommand();
            LoginStudentCommand();
            LoginTeacherCommand();
        }

        private void LoginCommand() {
            LogIn = new RelayCommand(LoginAction, CanExecuteLogin);
        }

        private void QuitCommand() {
            Quit = new RelayCommand(() => Application.Current.Shutdown());
        }

        
        /// ///////////////////////////////////////////////////////////////////////////
        

        private void LoginStudentCommand() {
            LoginStudent = new RelayCommand(() => {
                var user = User.GetByEmail("student2@mail.com");
                Login(user);
                //LoggedUser.Login(user);
                OnLoginSuccess?.Invoke();
            });
        }

        private void LoginTeacherCommand() {
            LoginTeacher = new RelayCommand(() => {
                var user = User.GetByEmail("teacher1@mail.eu");
                Login(user);
                //LoggedUser.Login(user);
                OnLoginSuccess?.Invoke();
            });
        }

        
        /// //////////////////////////////////////////////////////////////////////////////
        

        #endregion

        #region Action

        private void LoginAction() {
            if (Validate()) {
                var user = User.GetByEmail(Email);
                Login(user);
                //LoggedUser.Login(user);
                OnLoginSuccess?.Invoke();
           }
        }

        private bool CanExecuteLogin() {
            return Email != "Email" && Email != "";
        }

        #endregion

        #region Validation

        public override bool Validate() {
            ClearErrors();

            var user = User.GetByEmail(Email);
            if (!Validator.Exists(user)) { 
                AddError(nameof(Error), "Identifiant ou mot de passe incorrect");
            }
            else if (!Validator.ArePasswordsMatching(Password, user.Password)) {
                AddError(nameof(Error), "Identifiant ou mot de passe incorrect");
            }

            RaiseErrors();
            return !HasErrors;
        }

        #endregion

        protected override void OnRefreshData() {

        }

    }

}
