using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Moodle.Models {

    public class Student : User {

        #region Fields

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public virtual ICollection<Test> Tests { get; set; } = new HashSet<Test>();

        #endregion

        #region Constructors

        public Student(string fullName, string email, string password) : base(fullName, email, password) {
        }

        public Student() { }

        #endregion

        #region Getters

        public ObservableCollection<Subscription> GetSubscriptions() {
            return new ObservableCollection<Subscription>(Subscriptions);
        }

        public ObservableCollection<Course> GetCoursesToSubscribeTo() {
            var toSubscribeTo = Context.Courses
                                .Where(course => !Subscriptions.Select(subscription => subscription.Course).Contains(course));
            return new ObservableCollection<Course>(toSubscribeTo);
        }

        public Test GetTest(Quiz quiz) {
            return Tests.Where(t => t.Quiz == quiz && t.Student == this).FirstOrDefault();
        }

        public ObservableCollection<Subscription> GetSubscriptionsDb() {
            return new ObservableCollection<Subscription>(Context.Subscriptions.Where(s => s.Student == this));
        }

        #endregion

    }
}
