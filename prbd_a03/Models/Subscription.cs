using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moodle.Models {

    public class Subscription : EntityBase<Context> {

        #region Enum

        public enum Status {
            Pending,
            Active,
            Inactive
        }

        #endregion

        #region Fields

        [Key]
        public int StudentID { get; set; }
        [Key]
        public int CourseID { get; set; }

        [Required]
        public virtual Student Student { get; set; }
        [Required]
        public virtual Course Course { get; set; }

        private Status subscriptionStatus;
        public Status SubscriptionStatus {
            get => subscriptionStatus; 
            set { 
                subscriptionStatus = value;
                RaisePropertyChanged(nameof(SubscriptionStatus));
            }
        }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public bool IsInactive => SubscriptionStatus.Equals(Status.Inactive);
        public bool IsActive => !IsInactive;

        #endregion

        #region Constructors

        public Subscription(Student student, Course course, Status subscriptionStatus) {
            Student = student;
            Course = course;
            SubscriptionStatus = subscriptionStatus;
            student?.Subscriptions.Add(this);
            course?.Subscriptions.Add(this);
        }

        public Subscription() { }

        #endregion

        public int GetTeacherID() => Course.Teacher.UserID;

        public static Subscription Get(Course course, Student student) => Context.Subscriptions.SingleOrDefault(s => s.Course == course && s.Student == student);

    }

}