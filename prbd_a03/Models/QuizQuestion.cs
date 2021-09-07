using PRBD_Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class QuizQuestion : EntityBase<Context> {

        #region Fields

        [Key]
        public int QuizQuestionID { get; set; }

        public int Points { get; set; }

        [Required]
        public virtual Quiz Quiz { get; set; }
        [Required]
        public virtual Question Question { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
        public virtual ICollection<Result> Results { get; set; } = new HashSet<Result>();

        #endregion

        #region Constructors

        public QuizQuestion(Quiz quiz, Question question, int points) {
            Quiz = quiz;
            Question = question;
            Points = points;
            quiz?.QuizQuestions.Add(this);
            question?.QuizQuestions.Add(this);
        }

        public QuizQuestion(Question question) {
            Question = question;
        }

        public QuizQuestion() { }

        #endregion

    }

}