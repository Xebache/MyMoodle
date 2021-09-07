using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moodle.Models {

    public class Test : EntityBase<Context> {

        #region Fields

        [Key]
        public int TestID { get; set; }
        
        public DateTime StartedAt { get; set; }
        public int Result { get; set; } = 0;
        public bool Corrected { get; set; }

        [Required]
        public virtual Quiz Quiz { get; set; }
        [Required]
        public virtual Student Student { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
        public virtual ICollection<Result> Results { get; set; } = new HashSet<Result>();

        #endregion

        #region Constructor

        public Test(Student student, Quiz quiz, DateTime startedAt) {
            StartedAt = startedAt;
            Quiz = quiz;
            Student = student;
            quiz?.Tests.Add(this);
            student?.Tests.Add(this);
        }

        public Test() { }

        #endregion

        #region Method

        public static Test Get(string title, Student student) {
            return Context.Tests.SingleOrDefault(test => test.Quiz.Title == title && test.Student == student);
        }

        public ObservableCollection<Result> MakeCorrection() {
            ObservableCollection<Result> results = new(Results);

            if (!Corrected) {
                foreach (Answer a in Answers) {
                    QuizQuestion qq = a.QuizQuestion;
                    int points = qq.Points;
                    Result result = new(this, qq);

                    if (!a.Choice.Question.MultipleAnswers) {
                        if (a.Choice.Correct) {
                            result.Points = points;
                        }
                        else {
                            result.Points = 0;
                        }
                    }
                    else {
                        int nChoices = qq.Question.Choices.Where(c => c.Correct).Count();
                        int nbAnswers = Answers.Where(answer => answer.QuizQuestion.Equals(qq) && answer.Choice.Correct).Count();
                        if (nChoices == nbAnswers) {
                            result.Points = points;
                        }
                        else {
                            points = qq.Points / nChoices;
                            if (a.Choice.Correct) {
                                result.Points += points;
                            }
                        }
                    }
                    Result res = Results.Where(r => r.Test == this && r.QuizQuestion == qq).FirstOrDefault();
                    if(res != null) {
                        Results.Remove(res);
                        results.Remove(res);
                    }
                    results.Add(result);
                }

                int total = 0;
                foreach (Result result in results) {
                    total += result.Points;
                    if (!Context.Results.Contains(result))
                        Context.Results.Add(result);
                }
                Result = total;
                if (Quiz.EndAt.Date < DateTime.Now.Date) {
                    Corrected = true;
                }
                Context.Tests.Update(this);
                Context.SaveChanges();
            }
            return results;

        }

        #endregion

    }

}
