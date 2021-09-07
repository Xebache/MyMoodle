using Moodle.Models;
using PRBD_Framework;

namespace Moodle.ViewModels {

    class Tab_Teacher_CourseDetails_ViewModel : Common_ViewModel {

		#region Fields

		public Course_Edit_ViewModel CourseEditViewModel { get; set; } = new Course_Edit_ViewModel();
		public Course_Display_ViewModel CourseDisplayViewModel { get; set; } = new Course_Display_ViewModel();

		#endregion

		#region Constructor

		public Tab_Teacher_CourseDetails_ViewModel() { }

		#endregion

		protected override void OnRefreshData() { }

    }

}
