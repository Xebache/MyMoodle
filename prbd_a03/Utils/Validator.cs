using Moodle.Authentication;
using Moodle.Models;
using System.Text.RegularExpressions;

namespace Moodle.Utils {

    class Validator  {

        public static bool Exists(User user) {
            return user != null;
        }

        /// !!! STRING = " esp esp esp "   !!!!

        public static bool IsStringValid(string str) {
            return !string.IsNullOrEmpty(str) && str.Length >= 3;
        }

        public static bool IsEmailFormatValid(string email) {
            if (string.IsNullOrEmpty(email)) {
                return false;
            }
            Regex regex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return regex.Match(email).Success;
        }

        public static bool IsFullNameValid(string fullName) {
            return IsStringValid(fullName) && !fullName.Equals("Pseudo");
        }

        public static bool IsPasswordFormatValid(string password) {
            if (string.IsNullOrEmpty(password)) {
                return false;
            }
            Regex regex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&,])[A-Za-z\d@$!%*?&,]{8,}$");
            return regex.Match(password).Success;
        }

        public static bool ArePasswordsMatching(string password, string confirm) {
            return password.Equals(confirm);
        }

        public static bool EmailAlreadyTaken(string email) {
            var user = User.GetByEmail(email);
            return Exists(user) && LoggedUser.GetID() != user.UserID && email.Equals(user.Email);
        }

        public static bool IsScoreValid(int score) {
            return score > 0 && score <= 100;
        }

        

        

    }

}
