using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Quiz_Display_ViewModel : Common_ViewModel {

        #region Fields

        private Course _course;
        public Course Course {
            get => _course;
            set {
                SetProperty(ref _course, value);
            }
        }

        private Quiz _quizSelected;
        public Quiz QuizSelected {
            get => _quizSelected;
            set => SetProperty(ref _quizSelected, value, InitQuizToEdit);
        }

        // List of questions selected for the quiz
        private ObservableCollection<QuizQuestion> _quizQuestions = new();
        public ObservableCollection<QuizQuestion> QuizQuestions {
            get => _quizQuestions;
            set => SetProperty(ref _quizQuestions, value, () => Validate());
        }

        // QuizQuestion displayed in quiz form (a bit hacky...)
        private ObservableCollection<QuizQuestion> _selectedQuestion = new();
        public ObservableCollection<QuizQuestion> SelectedQuestion {
            get => _selectedQuestion;
            set => SetProperty(ref _selectedQuestion, value);
        }

        private bool _isEdit;
        public bool IsEdit {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private string _title;
        public string Title {
            get => _title;
            set {
                ClearErrors();
                SetProperty(ref _title, value);
            }
        }

        private int _score;
        public int Score {
            get => _score;
            set {
                ClearErrors();
                SetProperty(ref _score, value, () => Validate());
            }
        }

        public DateTime Now { get; set; } = DateTime.Now;


        private DateTime _startAt = DateTime.Now;
        public DateTime StartAt {
            get => _startAt;
            set => SetProperty(ref _startAt, value);
        }

        private DateTime _endAt = DateTime.Now;
        public DateTime EndAt {
            get => _endAt;
            set => SetProperty(ref _endAt, value);
        }

        public int Total {
            get {
                int total = 0;
                foreach (QuizQuestion quizQuestion in QuizQuestions)
                    total += quizQuestion.Points;
                return total;
            }
        }

        public ICommand QuizQuestionSelected { get; set; }

        #endregion

        #region Constructor

        public Quiz_Display_ViewModel() {
            CommandsManager();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            QuizQuestionSelectedCommand();
        }

        private void QuizQuestionSelectedCommand() {
            QuizQuestionSelected = new RelayCommand<QuizQuestion>(quizQuestion => {
                Console.WriteLine(quizQuestion.Question.Body);
                SelectedQuestion.Clear();
                SelectedQuestion.Add(quizQuestion);
                RaisePropertyChanged(nameof(SelectedQuestion));
            });
        }

        

        #endregion

        private void InitQuizToEdit() {
            if (_quizSelected != null) {
                IsEdit = true;
                QuizQuestions = new ObservableCollection<QuizQuestion>(QuizSelected.QuizQuestions);
                SelectedQuestion.Clear();
                //Question = null;
                Title = QuizSelected.Title;
                StartAt = QuizSelected.StartAt;
                EndAt = QuizSelected.EndAt;
                //Questions = Course.GetQuestionsForQuizToEdit(QuizSelected);
                RaisePropertyChanged();
            }
        }

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

        }

    }

}
