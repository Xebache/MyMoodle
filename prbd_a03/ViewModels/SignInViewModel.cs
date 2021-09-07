using Moodle.Authentication;
using Moodle.Models;
using Moodle.Utils;
using PRBD_Framework;
using System;
using System.Windows;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class SignInViewModel : Common_ViewModel {

        #region Fields

        private string _email = "Email";
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        private string _fullName = "Pseudo";
        public string FullName { get => _fullName; set => SetProperty(ref _fullName, value); }

        private string _password = "********";
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private string _confirm = "********";
        public string Confirm { get => _confirm; set => SetProperty(ref _confirm, value); }

        private string _error;
        public string Error { get => _error; set => SetProperty(ref _error, value); }

        public event Action OnSignInSuccess;

        public ICommand SignIn { get; set; }
        public ICommand Quit { get; set; }

        #endregion

        #region Constructor

        public SignInViewModel() : base() {
            CommandManager();
        }

        #endregion

        #region Commands

        private void CommandManager() {
            SignInCommand();
            QuitCommand();
        }

        private void SignInCommand() {
            SignIn = new RelayCommand(SignInAction, CanSignIn);
        }

        private void QuitCommand() {
            Quit = new RelayCommand(Application.Current.Shutdown);
        }

		#endregion

		#region Validation

		private void SignInAction() {
            if (Validate()) {
                var student = new Student(FullName, Email, Password);
                Context.Add(student);
                Context.SaveChanges();
                Login(student);
                OnSignInSuccess?.Invoke();
            }
        }

        private bool CanSignIn() {
            return Email != "Email" && FullName != "Pseudo" && Email != "" && FullName != "";
        }

        public override bool Validate() {
            ClearErrors();

            var user = User.GetByEmail(Email);
            if (Validator.Exists(user)) {
                AddError(nameof(Error), "Adresse mail déjà associée à un identifiant");
            }
            else if (!Validator.IsEmailFormatValid(Email)) {
                AddError(nameof(Error), "Format du mail invalide");
            }
            else if (!Validator.IsFullNameValid(FullName)) {
                AddError(nameof(Error), "Format du pseudo non invalide");
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

        #endregion

        protected override void OnRefreshData() {

        }
    }
}
