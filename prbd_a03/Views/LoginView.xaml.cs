using Moodle.Utils;
using PRBD_Framework;
using System.Windows;

namespace Moodle.Views {
    
    public partial class LoginView : WindowBase {

        #region Constructor

        public LoginView() {
            InitializeComponent();
        }

        #endregion

        #region Navigation

        private void BtnRegister_Click(object sender, RoutedEventArgs e) {
            ApplicationBase.NavigateTo<SignInView>();
        }

        private void Vm_OnLoginSuccess() {
            ApplicationBase.NavigateTo<DashBoardView>();
        }

        #endregion

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e) {
            txtEmail.SelectAll();
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e) {
            txtPassword.SelectAll();
        }

    }

}
