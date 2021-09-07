using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Moodle.Models {

    public class Teacher : User {

        #region Fields

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();

        #endregion

        #region Constructor

        public Teacher(string fullName, string email, string password) : base(fullName, email, password) { }

        public Teacher() { }

        #endregion

        #region Getters

        public ObservableCollection<Course> GetCourses() => new(Courses.OrderBy(c => c.Title));

        #endregion

    }

}