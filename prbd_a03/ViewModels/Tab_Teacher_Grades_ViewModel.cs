using Moodle.Models;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Tab_Teacher_Grades_ViewModel : Common_ViewModel {

        #region Fields

        private Course _course;
        public Course Course {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        private ObservableCollection<Quiz> _quiz = new();
        public ObservableCollection<Quiz> Quiz {
            get => _quiz;
            set => SetProperty(ref _quiz, value);
        }

        private Quiz _quizSelected;
        public Quiz QuizSelected {
            get => _quizSelected;
            set {
                SetProperty(ref _quizSelected, value);
                IsEnabled = true;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        private QuizQuestion _quizQuestionSelected = new();
        public QuizQuestion QuizQuestionSelected {
            get => _quizQuestionSelected;
            set => SetProperty(ref _quizQuestionSelected, value);
        }

        private bool _isEnabled = false;
        public bool IsEnabled {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private DataTable _dtTests;
        public DataTable DtTests {
            get => _dtTests;
            set => SetProperty(ref _dtTests, value);
        }

        public ICommand GoToCorrection { get; set; }


        #endregion

        #region Constructor

        public Tab_Teacher_Grades_ViewModel() : base() {
            CommandManager();
        }

        public void Init(Course course) {
            Course = course;
            OnRefreshData();
        }

        #endregion

        #region Command

        private void CommandManager() {
            GoToCorrectionCommand();
        }

        private void GoToCorrectionCommand() {
            GoToCorrection = new RelayCommand<Test>(test => 
                NotifyColleagues(AppMessages.VIEW_CORRECTION, test)
            );
        }

        #endregion

        private void MakeAllCorrections() {
            foreach (var test in Course.Quiz.SelectMany(quiz => quiz.Tests)) {
                test.MakeCorrection();
            }
        }

        #region DataTable

        private void BuildDtTests() {
            DtTests = new();

            int quizTotal = Course.Quiz.Select(q => q.Score).Sum();

            object[][] studentsTests = Course.MakeStudentsTestsArray();

            //columns
            DtTests.Columns.Add("Etudiants", typeof(string));
            foreach (var quiz in Course.Quiz) {
                DtTests.Columns.Add(quiz.Title + "(/" + quiz.Score + ")", typeof(int));
            }
            DtTests.Columns.Add("Total (/" + quizTotal.ToString() + ")", typeof(string));
            DtTests.Columns.Add("Moyenne (/100)", typeof(string));

            //rows
            for (int i = 0; i < studentsTests.Length; ++i) {
                DtTests.Rows.Add(studentsTests[i]);
            }
            RaisePropertyChanged(nameof(DtTests));
        }

        #endregion

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            Quiz = new ObservableCollection<Quiz>(Course.Quiz);
            RaisePropertyChanged(nameof(Quiz));
            MakeAllCorrections();
            BuildDtTests();
        }

    }

}
