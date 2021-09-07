using Moodle.Authentication;
using Moodle.Models;
using PRBD_Framework;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Menu_Teacher_NewCourse_ViewModel : Common_ViewModel {

		#region Fields

		private string _title = "Titre";
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public ICommand NewCourseTeacher { get; set; }

		#endregion

		#region Constructor

		public Menu_Teacher_NewCourse_ViewModel() : base() {
			NewCourseTeacherCommand();
		}

		protected override void OnRefreshData() {
			
		}

		#endregion

		#region Command

		private void NewCourseTeacherCommand() {
			NewCourseTeacher = new RelayCommand(NewCourseAction, CanExecute);
		}

		private void NewCourseAction() {
			AddNewCourse();
			Title = "Title";
			RaisePropertyChanged();
			NotifyColleagues(AppMessages.MSG_NEWCOURSE_TEACHER);
		}

		private void AddNewCourse() {
			Teacher teacher = (Teacher)LoggedUser;
			Course course = new(teacher, Title);
			Context.Add(course);
			Context.SaveChanges();
		}

		private bool CanExecute() {
			return Title != "Title" && Title != null;
		}
   
		#endregion


	}
}
