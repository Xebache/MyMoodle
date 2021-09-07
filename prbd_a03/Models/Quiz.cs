using Moodle.Authentication;
using Moodle.ViewModels;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class Quiz : EntityBase<Context> {

        #region Fields

        [Key]
        public int QuizID { get; set; }

        public String Title { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int Score { get; set; }

        [Required]
        public virtual Course Course { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new HashSet<QuizQuestion>();
        public virtual ICollection<Test> Tests { get; set; } = new HashSet<Test>();
        

        #endregion

        #region Constructors

        public Quiz(Course course, string title, DateTime startAt, DateTime endAt, int score, ICollection<QuizQuestion> questions) {
            Title = title;
            StartAt = startAt;
            EndAt = endAt;
            Score = score;
            Course = course;
            QuizQuestions = questions;
            course?.Quiz.Add(this);
        }

        public Quiz(Course course) {
            Course = course;
        }

        public Quiz() { }

        #endregion
        /*
        public Test GetTest() {
            Student student = (Student)Common_ViewModel.LoggedUser;
            Test test = student.GetTest(this);
            if (test == null) {
                test = new(student, this, DateTime.Now);
                Context.Tests.Add(test);
                Context.SaveChanges();
            }
            return test;
        }
        */

    }

}