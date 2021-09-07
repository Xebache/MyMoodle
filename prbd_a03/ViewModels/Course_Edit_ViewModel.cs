using Moodle.Authentication;
using Moodle.Models;
using Moodle.Utils;
using PRBD_Framework;
using System.Windows.Input;

namespace Moodle.ViewModels {
	class Course_Edit_ViewModel : Common_ViewModel {

        #region Fields

        private Course _course;
		public Course Course {
			get => _course;
			set => SetProperty(ref _course, value, OnRefreshData);
		}

		private string _title;
		public string Title {
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private string _description;
		public string Description {
			get => _description;
			set => SetProperty(ref _description, value);
		}

		private int _capacity;
		public int Capacity {
			get => _capacity;
			set => SetProperty(ref _capacity, value);
		}

		private bool _isEnabled = false;
		public bool IsEnabled {
			get => _isEnabled;
			set => SetProperty(ref _isEnabled, value);
		}

		private bool _isNew = false;
		public bool IsNew {
			get => _isNew;
			set => SetProperty(ref _isNew, value);
		}

		public ICommand Save { get; set; }
		public ICommand Cancel { get; set; }
		public ICommand Delete { get; set; }

        #endregion

        #region Constructor

        public Course_Edit_ViewModel() {
			CommandsManager();
		}

        #endregion

        #region Commands

        private void CommandsManager() {
			SaveCommand();
			CancelCommand();
			DeleteCommand();
			CourseToDisplayRegister();
			NewCourseRegister();
		}

		private void SaveCommand() {
			Save = new RelayCommand(SaveAction, CanSave);
		}

		private void CancelCommand() {
			Cancel = new RelayCommand(CancelAction, CanExecute);
		}

		private void DeleteCommand() {
			Delete = new RelayCommand(DeleteAction, CanDelete);
		}

		private void CourseToDisplayRegister() {
			Register<Course>(this, AppMessages.MSG_COURSE_TO_DISPLAY, course => {
				Course = course;
				IsNew = false;
				OnRefreshData();
			});
		}

		private void NewCourseRegister() {
			Register(this, AppMessages.MSG_CREATE_NEW_COURSE, () => {
				Course = new();
				IsNew = true;
				RaisePropertyChanged();
			});
        }

        #endregion

        #region Actions

        private void SaveAction() {
			if (Validate()) {
				UpdateCourse();
				if (IsNew) {
					Context.Add(Course);
				}
				Context.SaveChanges();
				CancelAction();
				NotifyColleagues(AppMessages.MSG_UPDATECOURSES_TEACHER);
			}
		}

		private void UpdateCourse() {
			Course.Title = Title;
			Course.Description = Description;
			Course.Capacity = Capacity;
			Course.Teacher = (Teacher)LoggedUser;
		}

		private bool CanSave() {
			return Course != null && !string.IsNullOrEmpty(Title);
		}

		private bool CanExecute() {
			return Title != null;
		}

		private void CancelAction() {
			Course = null;
			Title = "";
			Description = "";
			Capacity = 0;
			IsEnabled = false;
			RaisePropertyChanged();
		}

		private void DeleteAction() {
			Context.Remove(Course);
			Context.SaveChanges();
			NotifyColleagues(AppMessages.MSG_UPDATECOURSES_TEACHER);
			CancelAction();
		}

		private bool CanDelete() {
			if(!IsNew)
				return Course != null && Course.NbSubscriptions == 0;
			return false;
		}

        #endregion

        #region Validation

        public override bool Validate() {
			ClearErrors();

			if (!Validator.IsStringValid(Title)) {
				AddError(nameof(Title), "Titre invalide");
			}
			else if (Capacity < Course.NbSubscriptions) {
				AddError(nameof(Capacity), "Capacité plus petite que le nombre d'inscrits");
			}

			RaiseErrors();
			return !HasErrors;
		}

        #endregion

        protected override void OnRefreshData() {
			if(Course == null) {
				return;
            }
			if (Course == null) {
				Title = "";
				Description = "";
				Capacity = 0;
				IsEnabled = false;
				RaisePropertyChanged();
			}
			else {
				Title = Course.Title;
				Description = Course.Description;
				Capacity = Course.Capacity;
				IsEnabled = true;
				RaisePropertyChanged();
			}

		}

	}

}
