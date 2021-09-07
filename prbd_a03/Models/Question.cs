using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class Question : EntityBase<Context> {

        #region Fields

        [Key]
        public int QuestionID { get; set; }

        public String Body { get; set; }
        public bool MultipleAnswers { get; set; }
        public bool SimpleAnswer => !MultipleAnswers;
        //public bool Modifiable { get; set; } // ??? qd ds test ???

        [Required]
        public virtual Course Course { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual ICollection<Choice> Choices { get; set; } = new HashSet<Choice>();
        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new HashSet<QuizQuestion>();

        #endregion

        #region Constructors

        public Question(string body, bool multipleAnswers, Course course, HashSet<Category> categories) {
            Body = body;
            MultipleAnswers = multipleAnswers;
            Course = course;
            Categories = categories;
            course?.Questions.Add(this);
        }

        public Question() { 
        }

        #endregion

        public string ChoicesToString() {
            if (Choices.Count != 0) {
                string choices = "";
                foreach (Choice choice in Choices) {
                    choices += choice + "\n";
                }
                return choices[0..^1];
            }
            return "";
        }

    }

}
