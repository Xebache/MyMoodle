using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Tab_Student_Test_ViewModel : Common_ViewModel {

        #region Fields

        private Test _test;
        public Test Test { get => _test; set => SetProperty(ref _test, value); }

        private ObservableCollection<QuizQuestion> _quizQuestions = new();
        public ObservableCollection<QuizQuestion> QuizQuestions {
            get => _quizQuestions;
            set {
                SetProperty(ref _quizQuestions, value);
                
            }
        }

        private ObservableCollection<QuizQuestion> _selectedQuestion = new();
        public ObservableCollection<QuizQuestion> SelectedQuestion {
            get => _selectedQuestion;
            set { 
                SetProperty(ref _selectedQuestion, value);
            }
        }

        public ICommand TestQuestionSelected { get; set; }
        public ICommand ChoicesAdded { get; set; }
        public ICommand Save { get; set; }
        public ICommand ValidateTest { get; set; }

        #endregion

        #region Constructor

        public Tab_Student_Test_ViewModel() {
            CommandsManager();
        }

        public void Init(Test test) {
            Test = test;
            QuizQuestions = new ObservableCollection<QuizQuestion> (Test.Quiz.QuizQuestions);

            if (test.Answers.Count != 0) {
                foreach (var c in test.Answers.SelectMany(answer => 
                    QuizQuestions.SelectMany(qq => 
                        from Choice choice in qq.Question.Choices
                        where choice.ChoiceID == answer.ChoiceID
                        select choice))) { 
                    c.Selected = true; 
                }
            }  
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            TestQuestionSelectedCommand();
            ChoicesAddedCommand();
            SaveCommand();
            ValidateTestCommand();
        }

        private void TestQuestionSelectedCommand() {
            TestQuestionSelected = new RelayCommand<QuizQuestion>(quizQuestion => {
                SelectedQuestion.Clear();
                SelectedQuestion.Add(quizQuestion);
                RaisePropertyChanged(nameof(SelectedQuestion));
            });
        }

        private void ChoicesAddedCommand() {
            ChoicesAdded = new RelayCommand<IList<object>>(choices => {
                QuizQuestion qq = SelectedQuestion.FirstOrDefault();
                List<Choice> selectedChoices = qq.Question.Choices.Where(c => c.Selected).ToList();
                List<Choice> toUpdate = Test.Answers.Where(a => a.Choice.Question == qq.Question).Select(a => a.Choice).ToList();

                //add new answers
                foreach (Choice c in selectedChoices.Where(c => !toUpdate.Contains(c))) {
                    Test.Answers.Add(new Answer(Test, qq, c, DateTime.Now));
                }

                //remove old answers
                foreach (Choice c in toUpdate.Where(c => !selectedChoices.Contains(c))) {
                    Answer a = Test.Answers.Where(a => a.ChoiceID == c.ChoiceID).FirstOrDefault();
                    if (a != null)
                        Test.Answers.Remove(a);
                }
            });    
        }

        
        private void SaveCommand() {
            Save = new RelayCommand(() => SaveAction()); 
        }

        private void ValidateTestCommand() {
            ValidateTest = new RelayCommand<Test>(test => {
                Test.MakeCorrection();
                Context.Tests.Update(Test);
                Context.SaveChanges();
                NotifyColleagues(AppMessages.CLOSE_TAB_TEST, test);
            });
        }

        private void SaveAction() {
            foreach (Answer a in Test.Answers) {
                Console.WriteLine(a.Choice.Body);
                Console.WriteLine(Context.Answers.Contains(a));
                /*
                if (Context.Answers.Contains(a))
                    Context.Answers.Update(a);
                else
                    Context.Answers.Add(a);
                */
            }

            Context.SaveChanges();
        }

        #endregion

        protected override void OnRefreshData() {
            if (Test == null)
                return;

            Test = Test.Get(Test.Quiz.Title, (Student)LoggedUser);

            QuizQuestions = new ObservableCollection<QuizQuestion>(Test.Quiz.QuizQuestions);

            if (Test.Answers.Count != 0) {
                foreach (var c in Test.Answers.SelectMany(answer =>
                    QuizQuestions.SelectMany(qq =>
                        from Choice choice in qq.Question.Choices
                        where choice.ChoiceID == answer.ChoiceID
                        select choice))) {
                    c.Selected = true;
                }
            }
        }
    }
}
