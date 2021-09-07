using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Linq;

namespace Moodle.ViewModels {

    public class Tab_Student_TestCorrection_ViewModel : Common_ViewModel {

        #region Inner Classes

        public class CorrectedQuestion : EntityBase<Context> {

            public QuizQuestion QuizQuestion { get; set; }
            public ObservableCollection<CorrectedChoice> CorrectedChoices { get; set; } = new();
            public int Points { get; set; } 

            public CorrectedQuestion(QuizQuestion quizQuestion) {
                QuizQuestion = quizQuestion;
            }

        }

        public class CorrectedChoice : EntityBase<Context> {

            public Choice Choice { get; set; }

            private bool _isSelected;
            public bool IsSelected {
                get => _isSelected;
                set {
                    _isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }

            private bool _isFalse;
            public bool IsFalse {
                get => _isFalse;
                set {
                    _isFalse = value;
                    RaisePropertyChanged(nameof(IsFalse));
                }
            }

            public CorrectedChoice(Choice choice) {
                Choice = choice;
                IsSelected = false;
                IsFalse = false;
            }

        }

        #endregion

        #region Fields

        private Test _test;
        public Test Test { get => _test; set => SetProperty(ref _test, value); }

        private ObservableCollection<CorrectedQuestion> _corrections = new();
        public ObservableCollection<CorrectedQuestion> Corrections {
            get => _corrections;
            set => SetProperty(ref _corrections, value);
        }

        #endregion

        #region Constructor

        public Tab_Student_TestCorrection_ViewModel() {

        }

        public void Init(Test test) {
            Test = test;
            OnRefreshData();
        }

        #endregion

        private void BuildCorrections() {
            var Results = MakeCorrection();
            foreach (QuizQuestion quizQuestion in Test.Quiz.QuizQuestions) {
                CorrectedQuestion correctedQuestion = new(quizQuestion);
                ObservableCollection<CorrectedChoice> correction = new();
                foreach (Choice choice in quizQuestion.Question.Choices) {
                    CorrectedChoice cc = new(choice);
                    Answer answer = Test.Answers.FirstOrDefault(a => a.Choice.Equals(choice));
                    if (answer != null) {
                        cc.IsSelected = true;
                        if (!answer.Choice.Correct)
                            cc.IsFalse = true;
                    }
                    else if (!cc.IsSelected && choice.Correct)
                        cc.IsFalse = true;
                    correction.Add(cc);
                }
                correctedQuestion.CorrectedChoices = correction;
                correctedQuestion.Points = Results.Where(r => r.QuizQuestion == quizQuestion).Select(r => r.Points).FirstOrDefault();
                Corrections.Add(correctedQuestion);
            }
        }

        private ObservableCollection<Result> MakeCorrection() {
            var results = Test.MakeCorrection();
            RaisePropertyChanged(nameof(Test));
            return results;
        }

            
        protected override void OnRefreshData() {
            if (Test == null)
                return;

            Test = Test.Get(Test.Quiz.Title, (Student)LoggedUser);

            BuildCorrections();
        }
    }



}
