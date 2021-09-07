using Moodle.Models;
using Moodle.ViewModels;
using PRBD_Framework;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Moodle.Views {

    public partial class DashBoardView : WindowBase {

        #region Fields

        double leftPanelWidth;
        bool hiddenMenu;

        double updateUserHeight;
        bool hiddenUser;

        DispatcherTimer timerMenu;
        DispatcherTimer timerUser;

        #endregion

        #region Constructor

        public DashBoardView() {
            InitializeComponent();
            InitMenuEffect();
            hiddenUser = true;
        }

        #endregion

        #region Init Graphic Commponents

        private void InitMenuEffect() {
            InitTimers();
            InitData();
            //DataContext = new DashBoardViewModel();
        }

        private void InitTimers() {
            InitTimerMenu();
            InitTimerUser();
        }

        private void InitTimerMenu() {
            timerMenu = new DispatcherTimer {
                Interval = new TimeSpan(0, 0, 0, 0, 1)
            };
            timerMenu.Tick += TimerMenu_Tick;
        }

        private void InitTimerUser() {
            timerUser = new DispatcherTimer {
                Interval = new TimeSpan(0, 0, 0, 0, 1)
            };
            timerUser.Tick += TimerUser_Tick;
        }

        private void InitData() {
            leftPanelWidth = SidePanelLeft.Width;
            updateUserHeight = 110;
        }

        #endregion

        #region Graphic Components Effects

        //Drag MainWindow
        private void MainView_OnMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        //Translation Update User
        private void TimerUser_Tick(object sender, EventArgs e) {
            if (hiddenUser) {
                UpdateUserPanel.Height += 10;
                main.Height -= 10;
                if (UpdateUserPanel.Height >= updateUserHeight) {
                    timerUser.Stop();
                    hiddenUser = false;
                }
            }
            else {      
                UpdateUserPanel.Height -= 10;
                main.Height += 10;
                if (UpdateUserPanel.Height <= 0) {
                    timerUser.Stop();
                    hiddenUser = true;
                }
            }
        }

        //Translation ListView
        private void TimerMenu_Tick(object sender, EventArgs e) {
            if (hiddenMenu) {
                SidePanelLeft.Width += 10;
                SidePanelRight.Width -= 10;
                if (SidePanelLeft.Width >= leftPanelWidth) {
                    timerMenu.Stop();
                    hiddenMenu = false;
                }
            }
            else {
                SidePanelLeft.Width -= 10;
                SidePanelRight.Width += 10;
                if (SidePanelLeft.Width <= 0) {
                    timerMenu.Stop();
                    hiddenMenu = true;
                }
            }
        }

        private void Menu_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void Burger_Click(object sender, RoutedEventArgs e) {
            timerMenu.Start();
        }

        private void Login_Click(object sender, RoutedEventArgs e) {
            timerUser.Start();
        }

        #endregion

        #region Nav Tabs

        private void Vm_DisplayTab_Student_NewCourse() {
            var tab = tabControl.FindByHeader("Nouveau");
            if(tab == null)
                tabControl.Add(new Tab_Student_NewCourse_View(), "Nouveau");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Student_TestList(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Tests");
            if (tab == null)
                tabControl.Add(new Tab_Student_TestList_View(course), course.Title + " - Tests");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Student_Test(Test test) {
            Course course = test.Quiz.Course;
            var tab = tabControl.FindByHeader(course.Title + " - " + test.Quiz.Title);
            if (tab == null)
                tabControl.Add(new Tab_Student_Test_View(test), course.Title + " - " + test.Quiz.Title);
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Student_TestCorrection(Test test) {
            Course course = test.Quiz.Course;
            var tab = tabControl.FindByTag(course.Title + " - Correction " + test.Quiz.Title + " " + test.Student);
            if (tab == null)
                tabControl.Add(new Tab_Student_TestCorrection_View(test), course.Title + " - Correction " + test.Quiz.Title, course.Title + " - Correction " + test.Quiz.Title + " " + test.Student);
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Student_Grade(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Bulletin");
            if (tab == null)
                tabControl.Add(new Tab_Student_Grade_View(course), course.Title + " - Bulletin");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Student_Subscriptions() {
            var tab = tabControl.FindByHeader("Inscriptions");
            if (tab == null)
                tabControl.Add(new Tab_Student_Subscriptions_View(), "Inscriptions");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Student_CourseDetails(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Home");
            if(tab == null) {
                tabControl.Add(new Tab_Student_CourseDetails_View(course), course.Title + " - Home");
            }
            else {
                tabControl.SetFocus(tab);
            }
        }

        private void Vm_DisplayTab_Teacher_Questions(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Questions");
            if (tab == null)
                tabControl.Add(new Tab_Teacher_Questions_View(course), course.Title + " - Questions");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Teacher_Quiz(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Quiz");
            if (tab == null)
                tabControl.Add(new Tab_Teacher_Quiz_View(course), course.Title + " - Quiz");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Teacher_Grades(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Notes");
            if (tab == null)
                tabControl.Add(new Tab_Teacher_Grades_View(course), course.Title + " - Notes");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Teacher_Subscriptions(Course course) {
            var tab = tabControl.FindByHeader(course.Title + " - Inscriptions");
            if (tab == null)
                tabControl.Add(new Tab_Teacher_Subscriptions_View(course), course.Title + " - Inscriptions");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_DisplayTab_Teacher_CourseDetails() {
            var tab = tabControl.FindByHeader("Mes cours");
            if (tab == null)
                tabControl.Add(new Tab_Teacher_CourseDetails_View(), "Mes cours");
            else
                tabControl.SetFocus(tab);
        }

        private void Vm_CloseTab(TabItem tabitem) {
            tabControl.Items.Remove(tabitem);
        }

        private void Vm_Close_Tab_Test(Test test) {
            Course course = test.Quiz.Course;
            var tab = tabControl.FindByHeader(course.Title + " - " + test.Quiz.Title);
            if (tab != null)
                tabControl.Items.Remove(tab);
        }

        #endregion


    }

}
