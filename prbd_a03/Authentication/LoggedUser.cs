using Moodle.Models;
using PRBD_Framework;
using System;

namespace Moodle.Authentication {

    public sealed class LoggedUser : EntityBase<Context> {

        #region Fields

        private User _current;
        public User Current { get => _current; set => _current = value; }

        private Boolean _isStudent = false;
        public static Boolean IsStudent { get => Instance._isStudent; set => Instance._isStudent = value; }

        private Boolean _isTeacher = false;
        public static Boolean IsTeacher { get => Instance._isTeacher; set => Instance._isTeacher = value; }

        #endregion

        #region Singleton

        private static readonly Lazy<LoggedUser> lazy = new(() => new LoggedUser());

        public static LoggedUser Instance { get => lazy.Value; }

        private LoggedUser() { }

        #endregion

        #region Login / Logout

        private void _login(User user) {
            _current = user;
            if (user is Student) {
                _isStudent = true;
            }
            else
                _isTeacher = true;
        }

        public static void Login(User user) {
            Instance._login(user);
        }

        private void _logout() {
            _current = null;
            _isStudent = false;
            _isTeacher = false;
        }

        public static void Logout() {
            Instance._logout();
        }

        public static User GetLoggedUser() {
            return Instance.Current;
        }

        public static string GetFullName() {
            return GetLoggedUser().FullName;
        }

        public static int GetID() {
            return GetLoggedUser().UserID;
        }

        #endregion

    }

}
