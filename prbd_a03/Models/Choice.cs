using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class Choice : EntityBase<Context> {

        #region Fields

        [Key]
        public int ChoiceID { get; set; }

        public String Body { get; set; }
        public bool Correct { get; set; }
        public bool Selected { get; set; }

        [Required]
        public virtual Question Question { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        #endregion

        #region Constructors

        public Choice(string body, bool correct, Question question) {
            Body = body;
            Correct = correct;
            Question = question;
            question?.Choices.Add(this);
        }

        public Choice() {
        }

        #endregion

        public override string ToString() {
            return Correct ? "*" + Body : "" + Body;       
        }

        
    }

}