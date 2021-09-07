using Moodle.Authentication;
using Moodle.Models;
using Moodle.Views;
using PRBD_Framework;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class DashBoardViewModel : Common_ViewModel {

        #region Fields

        public String Logged => LoggedUser.FullName;

        public static bool IsStudent => IsLoggedUserStudent; 
        public static bool IsTeacher => IsLoggedUserTeacher; 

        public event Action DisplayTab_Student_NewCourse;
        public event Action<Course> DisplayTab_Student_TestList;
        public event Action<Course> DisplayTab_Student_Grade;
        public event Action DisplayTab_Student_Subscriptions;
        public event Action<Test> DisplayTab_Student_Test;
        public event Action<Test> DisplayTab_Student_TestCorrection;
        public event Action<Course> DisplayTab_Student_CourseDetails;
        public event Action<Course> DisplayTab_Teacher_Questions;
        public event Action<Course> DisplayTab_Teacher_Quiz;
        public event Action<Course> DisplayTab_Teacher_Grades;
        public event Action<Course> DisplayTab_Teacher_Subscriptions;
        public event Action DisplayTab_Teacher_CourseDetails;
        public event Action<TabItem> CloseTab;
        public event Action<Test> CloseTabTest;

        private int _selectedTab;
        public int SelectedTab { get => _selectedTab; set => SetProperty(ref _selectedTab, value); }

        public ICommand LogOut { get; set; }
        public ICommand Quit { get; set; }
        public ICommand QuitTab { get; set; }

        #endregion

        #region Constructor

        public DashBoardViewModel() : base() {
            //InitData();
            CommandsManager();
            ActionsManager();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            QuitCommand();
            LogoutCommand();
            QuitTabCommand();
            LoggedUpdated_Register();
        }

        private void QuitCommand() {
            Quit = new RelayCommand( Application.Current.Shutdown);
        }

        private void LogoutCommand() {
            LogOut = new RelayCommand(() => {
                Logout();
                ApplicationBase.NavigateTo<LoginView>();
            });
        }

        private void QuitTabCommand() {
            QuitTab = new RelayCommand<TabItem>(tabItem => {
                NotifyColleagues(AppMessages.CLOSE_TAB, tabItem);
            });
        }

        private void LoggedUpdated_Register() {
            Register(this, AppMessages.MSG_LOGGED_UPDATED, OnRefreshData);
        }

        #endregion

        #region Nav Actions

        private void ActionsManager() {
            Tab_Close_Register();
            Tab_Test_Close_Register();
            Tab_Student_NewCourse_Register();
            Tab_Student_TestList_Register();
            Tab_Student_Test_Register();
            Tab_Student_TestCorrection_Register();
            Tab_Student_Grade_Register();
            Tab_Student_Subscriptions_Register();
            Tab_Student_CourseDetails_Register();
            Tab_Teacher_Questions_Register();
            Tab_Teacher_Quiz_Register();
            Tab_Teacher_Grades_Register();
            Tab_Teacher_Subscriptions_Register();
            Tab_Teacher_CourseDetails_Register();
        }

        private void Tab_Close_Register() {
            Register<TabItem>(this, AppMessages.CLOSE_TAB, tabItem => 
                CloseTab?.Invoke(tabItem)
            );
        }

        private void Tab_Student_NewCourse_Register() {
            Register(this, AppMessages.VIEW_NEW_COURSE_STUDENT, () => 
                DisplayTab_Student_NewCourse?.Invoke()
            );
        }

        private void Tab_Student_TestList_Register() {
            Register<Course>(this, AppMessages.VIEW_TESTLIST_STUDENT, course => 
                DisplayTab_Student_TestList?.Invoke(course)
            );
        }

        private void Tab_Student_Test_Register() {
            Register<Test>(this, AppMessages.VIEW_NEW_TEST, test => 
                DisplayTab_Student_Test?.Invoke(test)
            );
        }

        private void Tab_Student_TestCorrection_Register() {
            Register<Test>(this, AppMessages.VIEW_CORRECTION, test => 
                DisplayTab_Student_TestCorrection?.Invoke(test)
            );
        }

        private void Tab_Student_Grade_Register() {
            Register<Course>(this, AppMessages.VIEW_GRADE_STUDENT, course => 
                DisplayTab_Student_Grade?.Invoke(course)
            );
        }

        private void Tab_Student_Subscriptions_Register() {
            Register(this, AppMessages.VIEW_SUBSCRIPTIONS_STUDENT, () => 
                DisplayTab_Student_Subscriptions?.Invoke()
            );
        }

        private void Tab_Student_CourseDetails_Register() {
            Register<Course>(this, AppMessages.VIEW_COURSE_DETAILS_STUDENT, course => 
                DisplayTab_Student_CourseDetails?.Invoke(course)
            );
        }

        private void Tab_Teacher_Questions_Register() {
            Register<Course>(this, AppMessages.VIEW_QUESTIONS_TEACHER, course => 
                DisplayTab_Teacher_Questions?.Invoke(course)
            );
        }

        private void Tab_Teacher_Quiz_Register() {
            Register<Course>(this, AppMessages.VIEW_QUIZ_TEACHER, course => 
                DisplayTab_Teacher_Quiz?.Invoke(course)
            );
        }

        private void Tab_Teacher_Grades_Register() {
            Register<Course>(this, AppMessages.VIEW_GRADES_TEACHER, course => 
                DisplayTab_Teacher_Grades?.Invoke(course)
            );
        }

        private void Tab_Teacher_Subscriptions_Register() {
            Register<Course>(this, AppMessages.VIEW_SUBSCRIPTIONS_TEACHER, course => 
                DisplayTab_Teacher_Subscriptions?.Invoke(course)
            );
        }

        private void Tab_Teacher_CourseDetails_Register() {
            Register(this, AppMessages.VIEW_COURSE_DETAILS_TEACHER, () => 
                DisplayTab_Teacher_CourseDetails?.Invoke()
            );
        }

        private void Tab_Test_Close_Register() { 
            Register<Test>(this, AppMessages.CLOSE_TAB_TEST, test => 
                CloseTabTest?.Invoke(test)
            );
        }

        #endregion

        protected override void OnRefreshData() {

        }

        
    }

}
