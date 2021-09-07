using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Tab_Student_TestList_ViewModel : Common_ViewModel {

        #region Fields

        private Course _course;
        public Course Course { get => _course; set => SetProperty(ref _course, value, OnRefreshData); }

        private ObservableCollection<Quiz> _quiz = new();
        public ObservableCollection<Quiz> Quiz {
            get => _quiz;
            set => SetProperty(ref _quiz, value);
        }

        public ICommand GoToTab { get; set; }
        public ICommand GoToCorrection { get; set; }

        #endregion

        #region Constructor

        public Tab_Student_TestList_ViewModel() {
            CommandsManager();
        }

        public void Init(Course course) {
            Course = course;
            OnRefreshData();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            NewQuiz_Register();
            GoToTabCommand();
            GoToCorrectionCommand();
        }

        private void NewQuiz_Register() {
            Register(this, AppMessages.MSG_NEW_QUIZ, () => {
                OnRefreshData();
            });
        }

        private void GoToTabCommand() {
            GoToTab = new RelayCommand<Quiz>(quiz => {
                DateTime today = DateTime.Today;
                if (DateTime.Compare(quiz.EndAt, today) >= 0) {
                    Test test = Test.Get(quiz.Title, (Student)LoggedUser);
                    NotifyColleagues(AppMessages.VIEW_NEW_TEST, test);
                }
                else {
                    Course course = quiz.Course;
                    NotifyColleagues(AppMessages.VIEW_GRADE_STUDENT, course);
                }
            });
        }

        private void GoToCorrectionCommand() {
            GoToCorrection = new RelayCommand<Quiz>(quiz => {
                Test test = Test.Get(quiz.Title, (Student)LoggedUser);
                NotifyColleagues(AppMessages.VIEW_CORRECTION, test);
            });
        }

        #endregion

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            //Quiz.RefreshFromModel(Course.Quiz);
            Quiz = new ObservableCollection<Quiz> (Course.Quiz);
        }
    }
}
