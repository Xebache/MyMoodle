using Moodle.Authentication;
using Moodle.Models;
using Moodle.Properties;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Moodle {

    public partial class App : ApplicationBase {

        public static User CurrentUser { get; private set; }

        public static bool IsTeacher => CurrentUser is Teacher;

        public static void Login(User user) {
            CurrentUser = user;
        }

        public static void Logout() {
            CurrentUser = null;
        }

        public static Context MoodleContext { get => Context<Context>(); }

        public App() {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            //RefreshDelay = Settings.Default.RefreshDelay;
            
            MoodleContext.Database.EnsureDeleted();
            MoodleContext.Database.EnsureCreated();
            MoodleContext.SeedData();
            
        }

        protected override void OnRefreshData() {
            if (CurrentUser?.Email != null)
                CurrentUser = User.GetByEmail(CurrentUser.Email);
            
        }

    }

}
