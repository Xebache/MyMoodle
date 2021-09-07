using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moodle.ViewModels {

    public abstract class Common_ViewModel : ViewModelBase<Context> {

        public static User LoggedUser => App.CurrentUser;

        public static void Login(User user) {
            App.Login(user);
        }

        public static void Logout() {
            App.Logout();
        }

        public static bool IsLoggedUserTeacher => App.IsTeacher;

        public static bool IsLoggedUserStudent => !App.IsTeacher;

        protected Common_ViewModel() : base() {
        }
    }

}
