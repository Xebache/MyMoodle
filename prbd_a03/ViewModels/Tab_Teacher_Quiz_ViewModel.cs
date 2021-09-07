using Moodle.Models;
using Moodle.Utils;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Moodle.ViewModels {
    public class Tab_Teacher_Quiz_ViewModel : Common_ViewModel {

        #region Fields

        public Questions_List_ViewModel QuestionsListViewModel { get; set; } = new Questions_List_ViewModel();
        //public Quiz_Display_ViewModel QuizDisplayViewModel { get; set; } = new Quiz_Display_ViewModel();

        // QUESTIONS LIST

        private Course _course;
        public Course Course {
            get => _course;
            set {
                SetProperty(ref _course, value);
            }
        }

        private ObservableCollection<Category> _categories = new();
        public ObservableCollection<Category> Categories {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        // Questions list to start with
        private ObservableCollection<Question> _questions = new();
        public ObservableCollection<Question> Questions {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }

        // Question selected to be added to QuizQuestions
        private Question _question;
        public Question Question {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        // QUIZ EDIT

        private ObservableCollection<Quiz> _quizToEdit = new();
        public ObservableCollection<Quiz> QuizToEdit {
            get => _quizToEdit;
            set => SetProperty(ref _quizToEdit, value);
        }

        private Quiz _quizSelected;
        public Quiz QuizSelected {
            get => _quizSelected;
            set => SetProperty(ref _quizSelected, value, InitQuizToEdit);
        }

        private bool _isEdit;
        public bool IsEdit {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        // QUIZ

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

        public ICommand Add { get; set; }
        public ICommand Remove { get; set; }
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        // View Actions
        public ICommand QuizQuestionSelected { get; set; }

        #endregion

        #region Constructor

        public Tab_Teacher_Quiz_ViewModel() : base() {
            CommandManager();
        }

        public void Init(Course course) {
            this.BindOneWay(nameof(Course), QuestionsListViewModel, nameof(QuestionsListViewModel.Course));
            this.BindTwoWays(nameof(Questions), QuestionsListViewModel, nameof(QuestionsListViewModel.Questions));
            this.BindOneWay(nameof(Categories), QuestionsListViewModel, nameof(QuestionsListViewModel.Categories));

            Course = course;
            Questions = new ObservableCollection<Question>(Course.Questions);
            //Categories = new ObservableCollection<Category>(Course.Categories);
            OnRefreshData();
        }

        #endregion

        #region Commmand

        private void CommandManager() {
            AddCommand();
            RemoveCommand();
            SaveCommand();
            CancelCommand();
            DeleteCommand();
            QuizQuestionSelectedCommand();
            QuestionEdited_Register();
            QuestionSelected_Register();
            QuestionDeleted_Register();
        }

        private void QuizQuestionSelectedCommand() {
            QuizQuestionSelected = new RelayCommand<QuizQuestion>(quizQuestion => {
                SelectedQuestion.Clear();
                SelectedQuestion.Add(quizQuestion);
            });
        }

        private void AddCommand() {
            Add = new RelayCommand(AddAction, CanAdd);
        }

        private void RemoveCommand() {
            Remove = new RelayCommand(RemoveAction, CanRemove);
        }

        private void SaveCommand() {
            Save = new RelayCommand(SaveAction, CanSave);
        }

        private void CancelCommand() {
            Cancel = new RelayCommand(CancelAction);
        }

        private void DeleteCommand() {
            Delete = new RelayCommand(DeleteAction, CanDelete);
        }

        private void QuestionEdited_Register() {
            Register<Question>(this, AppMessages.MSG_QUESTION_EDITED, question => {
                Questions = new ObservableCollection<Question>(Course.Questions);
                RaisePropertyChanged(nameof(Questions));

                //si la question est ds un quiz pas encore enregistré
                QuizQuestion listed = QuizQuestions.Where(qq => qq.Question.Equals(question)).FirstOrDefault();
                if (listed != null) {
                    QuizQuestion displayed = SelectedQuestion.FirstOrDefault();
                    if (displayed != null && displayed.Question == listed.Question) {
                        SelectedQuestion.Clear();
                        SelectedQuestion.Add(listed);
                    }
                }
            });
        }

        private void QuestionSelected_Register() {
            Register<Question>(this, AppMessages.MSG_DISPLAY_QUESTION_TEACHER, question => {
                Question = question;
            });
        }

        private void QuestionDeleted_Register() {
            Register<Question>(this, AppMessages.MSG_QUESTION_DELETED, question => {
                Questions = new ObservableCollection<Question>(Course.Questions);
                RaisePropertyChanged(nameof(Questions));

                //si la question est ds un quiz pas encore enregistré
                QuizQuestion listed = QuizQuestions.Where(qq => qq.Question.Equals(question)).FirstOrDefault();
                if (listed != null) {
                    QuizQuestions.Remove(listed);
                    QuizQuestion displayed = SelectedQuestion.FirstOrDefault();
                    if (displayed != null && displayed.Question == listed.Question) {
                        SelectedQuestion.Clear();
                    }
                }
            });
        }

        #endregion

        #region Actions

        private void AddAction() {
            QuizQuestion qq = new(Question);

            if (!QuizQuestions.Contains(qq)) {
                QuizQuestions.Add(qq);
                //......................................
                Questions.Remove(Question);
                RaisePropertyChanged(nameof(Questions));
            }
        }

        private bool CanAdd() {
            return Question != null && !QuizQuestions.Where(qq => qq.Question == Question).Any();
        }

        private void RemoveAction() {
            QuizQuestion quizQuestion = SelectedQuestion.FirstOrDefault();
            Question question = quizQuestion.Question;
            if (QuizQuestions.Contains(quizQuestion)) {
                QuizQuestions.Remove(quizQuestion);
                SelectedQuestion.Clear();
                //.......................................
                Questions.Add(question);
                RaisePropertyChanged(nameof(Questions));
            }
        }

        private bool CanRemove() {
            return SelectedQuestion.FirstOrDefault() != null;
        }


        private void SaveAction() {
            if (Validate()) {
                Quiz quiz;
                if (!IsEdit) {
                    quiz = new(Course, Title, StartAt, EndAt, Total, QuizQuestions.ToList());
                    Context.Add(quiz);
                }
                else {
                    QuizSelected.Title = Title;
                    QuizSelected.QuizQuestions = QuizQuestions.ToList();
                    QuizSelected.StartAt = StartAt;
                    QuizSelected.EndAt = EndAt;
                }

                Context.SaveChanges();

                NotifyColleagues(AppMessages.MSG_QUESTIONS_UNMODIFIABLE);
                NotifyColleagues(AppMessages.MSG_NEW_QUIZ);
                QuizToEdit.RefreshFromModel(Course.QuizToEdit());
                CancelAction();
            }
        }

        private bool CanSave() {
            return Title != "" && DateTime.Compare(StartAt, EndAt) <= 0 && Total != 0 && QuizQuestions.Count > 0;
        }

        private void CancelAction() {
            Questions = new ObservableCollection<Question>(Course.Questions);
            QuizQuestions.Clear();
            SelectedQuestion.Clear();
            Question = null;
            Title = "";
            StartAt = DateTime.Now;
            EndAt = DateTime.Now;
            IsEdit = false;
        }

        private void DeleteAction() {
            Context.Remove(QuizSelected);
            Context.SaveChanges();
            NotifyColleagues(AppMessages.MSG_QUESTIONS_UNMODIFIABLE);
            NotifyColleagues(AppMessages.MSG_NEW_QUIZ);
            CancelAction();
            OnRefreshData();
        }

        private bool CanDelete() {
            return IsEdit;
        }

        private void InitQuizToEdit() {
            if (_quizSelected != null) {
                IsEdit = true;
                QuizQuestions = new ObservableCollection<QuizQuestion>(QuizSelected.QuizQuestions);
                SelectedQuestion.Clear();
                Question = null;
                Title = QuizSelected.Title;
                StartAt = QuizSelected.StartAt;
                EndAt = QuizSelected.EndAt;
                Questions = Course.GetQuestionsForQuizToEdit(QuizSelected);
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Validation

        public override bool Validate() {
            ClearErrors();

            if (!Validator.IsStringValid(Title)) {
                AddError(nameof(Title), "Titre invalide");
            }
            if ((IsEdit && Title != QuizSelected.Title && Course.Quiz.Where(q => q.Title == Title).FirstOrDefault() != null)
                || (!IsEdit && Course.Quiz.Where(q => q.Title == Title).FirstOrDefault() != null)) {
                AddError(nameof(Title), "Un Quiz avec le même titre existe déjà");
            }

            foreach (var qq in QuizQuestions.Where(qq => !Validator.IsScoreValid(qq.Points))) {
                AddError(nameof(QuizQuestions), "Points de 1 à 100 pour chaque question");
            }


            RaiseErrors();
            return !HasErrors;
        }

        #endregion

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            //Questions = new ObservableCollection<Question>(Course.Questions);
            Categories = new ObservableCollection<Category>(Course.Categories);
            QuizToEdit = Course.QuizToEdit();


        }

    }

}
