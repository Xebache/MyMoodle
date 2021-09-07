using Moodle.Utils;
using PRBD_Framework;

namespace Moodle.Views {
    
    public partial class SignInView : WindowBase {

        #region Constructor

        public SignInView() {
            InitializeComponent();
        }

        #endregion

        #region Navigation

        private void BtnBack_Click(object sender, System.Windows.RoutedEventArgs e) {
            ApplicationBase.NavigateTo<LoginView>();
        }

        private void Vm_OnSignInSuccess() {
            ApplicationBase.NavigateTo<DashBoardView>();
        }

        #endregion

    }

}
