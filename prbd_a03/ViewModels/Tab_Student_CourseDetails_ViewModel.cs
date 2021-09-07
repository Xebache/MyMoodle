using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {
    public class Tab_Student_CourseDetails_ViewModel : Common_ViewModel {

        private Course _course;
        public Course Course { get => _course; set => SetProperty(ref _course, value); }

        private Subscription _subscription;
        public Subscription  Subscription { get => _subscription; set => SetProperty(ref _subscription, value); }

        private ObservableCollection<DateTime> _quizDates = new();
        public ObservableCollection<DateTime> QuizDates {
            get => _quizDates;
            set => SetProperty(ref _quizDates, value);
        }

        public ICommand QuizStudent { get; set; }
        public ICommand GradeStudent { get; set; }

        public Tab_Student_CourseDetails_ViewModel() {
            CommandsManager();
        }

        public void Init(Course course) {
            Course = course;
            OnRefreshData();
        }

        private void CommandsManager() {
            QuizStudent_Command();
            GradeStudent_Command();
        }

        private void QuizStudent_Command() {
            QuizStudent = new RelayCommand(() => NotifyColleagues(AppMessages.VIEW_TESTLIST_STUDENT, Course));
        }

        private void GradeStudent_Command() {
            GradeStudent = new RelayCommand(() => NotifyColleagues(AppMessages.VIEW_GRADE_STUDENT, Course));
        }

        private void BuildQuizDates() {
            foreach (Quiz q in Course.Quiz) {
                for (DateTime date = q.StartAt; date <= q.EndAt; date = date.AddDays(1)) {
                    QuizDates.Add(date);
                }
            }
            RaisePropertyChanged(nameof(QuizDates));
        }

        protected override void OnRefreshData() {
            if (Course == null) {
                return;
            }

            Course = Course.GetByTitle(Course.Title);

            if (IsLoggedUserStudent) {
                Student student = (Student)LoggedUser;
                Subscription = Subscription.Get(Course, student);
            }

            BuildQuizDates();

            RaisePropertyChanged();
        }

    }

}
