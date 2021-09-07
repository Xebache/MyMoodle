using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moodle.Models {

    public class Course : EntityBase<Context> {

        #region Fields

        [Key]
        public int CourseID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }

        [Required]
        public virtual Teacher Teacher { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public virtual ICollection<Quiz> Quiz { get; set; } = new HashSet<Quiz>();
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();

        public int NbActiveStudents => Subscriptions.Count(s => s.SubscriptionStatus == Subscription.Status.Active);
        public int NbPendingStudents => Subscriptions.Count(s => s.SubscriptionStatus == Subscription.Status.Pending);
        public int NbInactiveStudents => Subscriptions.Count(s => s.SubscriptionStatus == Subscription.Status.Inactive);
        public int NbSubscriptions => NbActiveStudents + NbPendingStudents;
        public int NbAvailable => Capacity - NbSubscriptions;
        public bool IsComplete => NbAvailable <= 0;

        #endregion

        #region Constructors

        public Course(Teacher teacher, string title) {
            Title = title;
            Teacher = teacher;
            teacher?.Courses.Add(this);
        }

        public Course(Teacher teacher, string title, int capacity) {
            Title = title;
            Teacher = teacher;
            Capacity = capacity;
            teacher?.Courses.Add(this);
        }

        public Course(Teacher teacher, string title, int capacity, string description) {
            Title = title;
            Teacher = teacher;
            Capacity = capacity;
            Description = description;
            teacher?.Courses.Add(this);
        }

        public Course() {
            
        }

        #endregion

        #region Queries

        public static Course GetByTitle(string title) {
            return Context.Courses.SingleOrDefault(course => course.Title == title);
        }

        public ObservableCollection<Subscription> GetFilteredSubscriptions(Subscription.Status Filter) {
            var filtered = Subscriptions.Where(s => s.SubscriptionStatus == Filter);
            return new ObservableCollection<Subscription>(filtered);
        }

        public ObservableCollection<Subscription> GetAllSubscriptions() {
            return new ObservableCollection<Subscription> (Subscriptions);
        }

        public ObservableCollection<User> GetStudentsToAdd() {
            var toAdd = Context.Users.Where(student => !Subscriptions.Select(subscription => subscription.Student).Contains(student) && student is Student);
            return new ObservableCollection<User>(toAdd);
        }

        public ObservableCollection<Question> QuestionsToEdit() {
            var cantEdit = from q in Quiz
                         from qu in q.QuizQuestions
                         where q.StartAt.Date < DateTime.Now.Date
                         select qu.Question;
            foreach (Question q in cantEdit) {
                Console.WriteLine(q.Body);
            }
            var toEdit = Questions.Except(cantEdit);
            return new ObservableCollection<Question>(toEdit);
        }
        
        public object[][] MakeStudentsTestsArray() {
            object[][] studentsTests = new object[Subscriptions.Count][];

            int quizSum = Quiz.Select(quiz => quiz.Score).Sum();

            Student[] Students = Subscriptions.Select(subscription => subscription.Student).OrderBy(student => student.FullName).ToArray();

            for (int i = 0; i < Students.Length; ++i) {
                //student
                studentsTests[i] = new object[] { Students[i].FullName };
                int total = 0;
                //student with tests
                foreach (var t in Quiz.SelectMany(quiz => quiz.Tests.Where(test => test.Student.Equals(Students[i])))) {
                    total += t.Result;
                    studentsTests[i] = studentsTests[i].Concat(new object[] { t.Result }).ToArray();
                }
                //student with no test
                foreach (var q in Quiz.Where(quiz => !quiz.Tests.Select(test => test.Student).Contains(Students[i]))) {
                    studentsTests[i] = studentsTests[i].Concat(new object[] { 0 }).ToArray();
                }
                studentsTests[i] = studentsTests[i].Concat(new object[] { total, (total * 100) / quizSum }).ToArray();
            }
            return studentsTests;
        }

        public ObservableCollection<Quiz> QuizToEdit() {
            return new ObservableCollection<Quiz>(Quiz.Where(q => q.StartAt.Date > DateTime.Now.Date));
        }

        public ObservableCollection<Question> GetQuestionsForQuizToEdit(Quiz quiz) {
            return new ObservableCollection<Question>(Questions.Except(quiz.QuizQuestions.Select(qq => qq.Question)));
        }

        #endregion

    }

}