using PRBD_Framework;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class Result : EntityBase<Context> {

        #region Fields

        [Key]
        public int ResultID { get; set; }

        public int Points { get; set; }

        [Required]
        public virtual QuizQuestion QuizQuestion { get; set; }
        [Required]
        public virtual Test Test { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        #endregion

        #region Constructors

        public Result() { }

        public Result(Test test, QuizQuestion quizQuestion) {
            Test = test;
            QuizQuestion = quizQuestion;
        }

        public Result(Test test, QuizQuestion quizQuestion, int points) {
            Test = test;
            QuizQuestion = quizQuestion;
            Points = points;
            test?.Results.Add(this);
            quizQuestion?.Results.Add(this);
        }

        #endregion

    }
}
