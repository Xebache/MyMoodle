using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Tab_Student_Grade_ViewModel : Common_ViewModel {

        #region Fields

        public static Student Student => (Student)LoggedUser;

        private Course _course;
        public Course Course { get => _course; set => SetProperty(ref _course, value); }

        private ObservableCollection<Test> _tests = new();
        public ObservableCollection<Test> Tests {
            get => _tests;
            set => SetProperty(ref _tests, value);
        }

        private DataTable _dtTests;
        public DataTable DtTests {
            get => _dtTests;
            set {
                SetProperty(ref _dtTests, value);
                RaisePropertyChanged(nameof(DtTests));
            }
        }

        public ICommand GoToCorrection { get; set; }

        #endregion

        #region Constructor

        public Tab_Student_Grade_ViewModel() {
            CommandManager();
        }

        public void Init(Course course) {
            Course = course;
            RaisePropertyChanged(nameof(Course));
            OnRefreshData();
        }

        #endregion

        #region Command

        private void CommandManager() {
            GoToCorrectionCommand();
        }

        private void GoToCorrectionCommand() {
            GoToCorrection = new RelayCommand<DataRowView>(drv => {
                string title = (string)drv[0];
                Test test = Student.Tests.Where(t => t.Quiz.Title.Equals(title)).FirstOrDefault();
                NotifyColleagues(AppMessages.VIEW_CORRECTION, test);
            }
            );
        }

        #endregion

        #region Datatable

        public void MakeDataTable() {

            var tests = (from quiz in Course.Quiz
                       join t in Student.Tests on quiz equals t.Quiz into testStudent
                       from ts in testStudent.DefaultIfEmpty()
                       select new { quiz.Title, quiz.StartAt, res = ts?.Result ?? 0, quiz.Score }).ToArray();

            DtTests = new();

            //columns
            DtTests.Columns.Add("Titre", typeof(string));
            DtTests.Columns.Add("Date", typeof(string));
            DtTests.Columns.Add("Résultat", typeof(string));
            DtTests.Columns.Add("Total", typeof(string));

            //rows
            foreach (var x in tests) {
                var row = DtTests.NewRow();
                row[0] = x.Title;
                row[1] = x.StartAt.ToShortDateString();
                row[2] = x.res;
                row[3] = x.Score;
                DtTests.Rows.Add(row);
            }

            int total = Course.Quiz.Where(q => q.EndAt.Date < DateTime.Now.Date).Select(q => q.Score).Sum();
            int sum = Student.Tests.Select(t => t.Result).Sum();

            var rowBis = DtTests.NewRow();
            rowBis[0] = "Total";
            rowBis[1] = "";
            rowBis[2] = sum;
            rowBis[3] = total;
            DtTests.Rows.Add(rowBis);

            var rowTer = DtTests.NewRow();
            rowTer[0] = "Moyenne";
            rowTer[1] = "";
            rowTer[2] = (sum * 100) / total;
            rowTer[3] = "100";
            DtTests.Rows.Add(rowTer);

            RaisePropertyChanged(nameof(DtTests));
        }

        #endregion

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            Tests = new ObservableCollection<Test>(Student.Tests);
            foreach (Test test in Tests) {
                test.MakeCorrection();
            }
            RaisePropertyChanged(nameof(Tests));
            MakeDataTable();
        }

    }

}
