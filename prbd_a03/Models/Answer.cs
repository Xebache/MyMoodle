using PRBD_Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class Answer : EntityBase<Context> {

        #region Fields

        [Key]
        public int TestID { get; set; }
        [Key]
        public int QuizQuestionID { get; set; }
        [Key]
        public int ChoiceID { get; set; }

        public DateTime AnsweredAt { get; set; }

        [Required]
        public virtual Test Test { get; set; }
        [Required]
        public virtual Choice Choice { get; set; }
        [Required]
        public virtual QuizQuestion QuizQuestion { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        #endregion

        #region Constructors

        public Answer() { }

        public Answer(Test test, QuizQuestion quizQuestion, Choice choice, DateTime answeredAt) {
            Test = test;
            QuizQuestion = quizQuestion;
            Choice = choice;
            AnsweredAt = answeredAt;
            
            test?.Answers.Add(this);
           // choice?.Answers.Add(this);
            quizQuestion?.Answers.Add(this);
            
        }

        #endregion

    }

}
