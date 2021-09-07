using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;
using System;
using System.Collections.Generic;

namespace Moodle.Models {

    public class Context : DbContextBase {

        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => {
            builder.AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=prbd_a03")
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(_loggerFactory)
                .UseLazyLoadingProxies(true);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Answer>().HasKey(a => new { a.TestID, a.ChoiceID, a.QuizQuestionID });
            modelBuilder.Entity<Subscription>().HasKey(s => new { s.StudentID, s.CourseID });


            //CHOICE
            modelBuilder.Entity<Choice>()
                .HasMany(choice => choice.Answers)
                .WithOne(answer => answer.Choice)
                .OnDelete(DeleteBehavior.ClientCascade);


            //COURSE
            modelBuilder.Entity<Course>()
                .HasMany(course => course.Subscriptions)
                .WithOne(subscription => subscription.Course)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Course>()
                .HasMany(course => course.Categories)
                .WithOne(category => category.Course)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasMany(course => course.Quiz)
                .WithOne(quiz => quiz.Course)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasMany(course => course.Questions)
                .WithOne(question => question.Course)
                .OnDelete(DeleteBehavior.ClientCascade);


            //QUESTION
            modelBuilder.Entity<Question>()
                .HasMany(question => question.Choices)
                .WithOne(choice => choice.Question)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasMany(question => question.QuizQuestions)
                .WithOne(quizQuestion => quizQuestion.Question)
                .OnDelete(DeleteBehavior.Cascade);


            //QUIZ
            modelBuilder.Entity<Quiz>()
                .HasMany(quiz => quiz.Tests)
                .WithOne(test => test.Quiz)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quiz>()
                .HasMany(quiz => quiz.QuizQuestions)
                .WithOne(quizQuestion => quizQuestion.Quiz)
                .OnDelete(DeleteBehavior.Cascade);

            
            //QUIZQUESTION
            modelBuilder.Entity<QuizQuestion>()
                .HasMany(quizQuestion => quizQuestion.Results)
                .WithOne(result => result.QuizQuestion)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<QuizQuestion>()
                .HasMany(quizQuestion => quizQuestion.Answers)
                .WithOne(answer => answer.QuizQuestion)
                .OnDelete(DeleteBehavior.ClientCascade);
            

            //STUDENT
            modelBuilder.Entity<Student>()
                .HasMany(student => student.Subscriptions)
                .WithOne(subscription => subscription.Student)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Student>()
                .HasMany(student => student.Tests)
                .WithOne(test => test.Student)
                .OnDelete(DeleteBehavior.ClientCascade);


            //TEACHER
            modelBuilder.Entity<Teacher>()
                .HasMany(teacher => teacher.Courses)
                .WithOne(course => course.Teacher)
                .OnDelete(DeleteBehavior.Cascade);


            //TEST
            modelBuilder.Entity<Test>()
                .HasMany(test => test.Answers)
                .WithOne(answer => answer.Test)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Test>()
                .HasMany(test => test.Results)
                .WithOne(result => result.Test)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        

        public void SeedData() {
            Database.BeginTransaction();

            Teacher teacher1 = new("Prof 1", "teacher1@mail.eu", "Password1,");
            Teacher teacher2 = new("Prof 2", "teacher2@mail.eu", "Password1,");
            Teacher teacher3 = new("Prof 3", "teacher3@mail.eu", "Password1,");
            Teacher teacher4 = new("Prof 4", "teacher4@mail.eu", "Password1,");
            Teachers.AddRange(new[] { teacher1, teacher2, teacher3, teacher4 });
            SaveChanges();

            Student student1 = new("Etudiant 1", "student1@mail.com", "Password1,");
            Student student2 = new("Etudiant 2", "student2@mail.com", "Password1,");
            Student student3 = new("Etudiant 3", "student3@mail.com", "Password1,");
            Student student4 = new("Etudiant 4", "student4@mail.com", "Password1,");
            Student student5 = new("Etudiant 5", "student5@mail.com", "Password1,");
            Student student6 = new("Etudiant 6", "student6@mail.com", "Password1,");
            Students.AddRange(new[] { student1, student2, student3, student4, student5, student6 });
            SaveChanges();

            string descriptionMap = "Bienvenue en MAP! : la boîte à outils théoriques pour une meilleure compréhension des principes de la programmation.\nDes maths, plein d'exos et du fun du fun du fun !!!";
            string descriptionSnet = "Le hacking pour les nuls...";

            Course prw2 = new(teacher1, "PRW2", 0);
            Course anc3 = new(teacher4, "ANC3", 5);
            Course prbd = new(teacher1, "PRBD", 5);
            Course snet = new(teacher2, "SNET", 3, descriptionSnet);
            Course map = new(teacher1, "MAP", 6, descriptionMap);
            Courses.AddRange(new[] {
                prw2, anc3, prbd, snet
            });
            SaveChanges();

            Subscription s1 = new(student2, anc3, Subscription.Status.Active);
            Subscription s2 = new(student2, prbd, Subscription.Status.Pending);
            Subscription s3 = new(student2, snet, Subscription.Status.Inactive);
            Subscription s4 = new(student3, prbd, Subscription.Status.Active);
            Subscription s5 = new(student4, prbd, Subscription.Status.Active);
            Subscription s6 = new(student5, prbd, Subscription.Status.Active);
            Subscription s7 = new(student1, prbd, Subscription.Status.Active);
            Subscription s8 = new(student3, map, Subscription.Status.Active);
            Subscription s9 = new(student4, map, Subscription.Status.Pending);
            Subscription s10 = new(student5, map, Subscription.Status.Pending);
            Subscription s11 = new(student2, map, Subscription.Status.Active);
            Subscriptions.AddRange(new[] { s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11 });
            SaveChanges();

            Category cat1 = new("Logique", map);
            Category cat2 = new("Math", map);
            Category cat3 = new("Géométrie", map);
            Category cat4 = new("Drôle", map);
            Category cat5 = new("Algèbre", map);
            Categories.AddRange(new[] { cat1, cat2, cat3, cat4, cat5 });
            SaveChanges();

            Question q1 = new("A.(-A + B) = ?", false, map, new HashSet<Category> { cat1 });
            Question q2 = new("A.(A + B) = A", false, map, new HashSet<Category> { cat1, cat2 });
            Question q3 = new("Que peut-on déduire de la proposition suivante : \"Il pleut, alors j'utilise mon parapluie\" ?", true, map, new HashSet<Category> { cat2 });
            Question q4 = new("Que peut-on dire du carré ?", true, map, new HashSet<Category> { cat3 });
            Question q5 = new("Quel est le périmètre du rectangle ?", false, map, new HashSet<Category> { cat3 });
            Question q6 = new("Quel âge avait Rimbaud ?", true, map, new HashSet<Category> { cat1, cat4 });
            Question q7 = new("14 + 28 =", false, map, new HashSet<Category> { cat5 });
            Questions.AddRange(new[] { q1, q2, q3, q4, q5, q6, q7 });
            SaveChanges();

            
            Choice c1 = new("A", false, q1);
            Choice c2 = new("A . B", true, q1);
            Choice c3 = new("(A + B) . (A + C)", false, q1);
            Choice c4 = new("Aucune de ces propositions", false, q1);
            Choice c5 = new("Vrai", true, q2);
            Choice c6 = new("Faux", false, q2);
            Choice c7 = new("La proposition est vraie s'il ne pleut pas", true, q3);
            Choice c8 = new("La proposition est toujours vraie s'il pleut", false, q3);
            Choice c9 = new("La proposition est vraie si je n'utilise pas mon parapluie quand il pleut", false, q3);
            Choice c10 = new("La proposition est vraie lorsque j'utilise mon parapluie quand il ne pleut pas", true, q3);
            Choice c11 = new("Il a quatre côtés de même longueur", true, q4);
            Choice c12 = new("Sa surface vaut quatre fois le côté", false, q4);
            Choice c13 = new("Il a un angle droit", true, q4);
            Choice c14 = new("C'est un trapèze", true, q4);
            Choice c15 = new("(L X l)", false, q5);
            Choice c16 = new("(L X 4)", false, q5);
            Choice c17 = new("(L X l) / 2", false, q5);
            Choice c18 = new("(L + l) / 2", true, q5);
            Choice c19 = new("A noir", false, q6);
            Choice c20 = new("Euh...", true, q6);
            Choice c21 = new("E blanc", false, q6);
            Choice c22 = new("Verlaine - Rimbaud = Baudelaire au carré", false, q6);
            Choice c23 = new("42", true, q7);
            Choice c24 = new("52", false, q7);
            Choice c25 = new("43", false, q7);
            Choice c26 = new("33", false, q7);
            Choices.AddRange(new[] { c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17, c18, c19, c20, 
                c21, c22, c23, c24, c25, c26 });
            SaveChanges();

            Quiz quiz1 = new(map);
            
            QuizQuestion qq1 = new(quiz1, q4, 25);
            QuizQuestion qq2 = new(quiz1, q5, 25);
            QuizQuestion qq3 = new(quiz1, q6, 25);
            QuizQuestion qq4 = new(quiz1, q7, 25);
            QuizQuestions.AddRange(new[] { qq1, qq2, qq3, qq4 });

            quiz1.Title = "Premier test";
            quiz1.StartAt = new DateTime(2021, 3, 29);
            quiz1.EndAt = new DateTime(2021, 4, 4);
            quiz1.Score = 100;
            quiz1.QuizQuestions = new HashSet<QuizQuestion> { qq1, qq2, qq3, qq4 };
            Quiz.Add(quiz1);
            SaveChanges();

            Quiz quiz2 = new(map);

            QuizQuestion qq5 = new(quiz2, q7, 50);

            QuizQuestions.Add(qq5);

            quiz2.Title = "Deuxième test";
            quiz2.StartAt = new DateTime(2021, 5, 3);
            quiz2.EndAt = new DateTime(2021, 5, 5);
            quiz2.Score = 50;
            quiz2.QuizQuestions = new HashSet<QuizQuestion> { qq5 };
            Quiz.Add(quiz2);
            SaveChanges();

            Quiz quiz3 = new(map);

            QuizQuestion qq6 = new(quiz3, q7, 50);
            QuizQuestion qq7 = new(quiz3, q2, 50);
            QuizQuestions.AddRange(new[] { qq6, qq7 });

            quiz3.Title = "Troisième test";
            quiz3.StartAt = new DateTime(2021, 6, 3);
            quiz3.EndAt = new DateTime(2021, 6, 6);
            quiz3.Score = 100;
            quiz3.QuizQuestions = new HashSet<QuizQuestion> { qq6, qq7 };
            Quiz.Add(quiz3);
            SaveChanges();

            Test test1 = new(student2, quiz1, new DateTime(2021, 3, 30));
            Test test2 = new(student3, quiz1, new DateTime(2021, 4, 1));
            Test test3 = new(student2, quiz2, new DateTime(2021, 5, 4));
            Tests.AddRange(new[] { test1, test2, test3 });
            SaveChanges();

            Answer a1 = new(test1, qq3, c22, new DateTime(2021, 3, 30));
            Answer a2 = new(test1, qq4, c23, new DateTime(2021, 3, 30));
            Answer a3 = new(test1, qq1, c11, new DateTime(2021, 3, 30));
            Answer a4 = new(test1, qq1, c12, new DateTime(2021, 3, 30));
            Answer a5 = new(test2, qq3, c21, new DateTime(2021, 4, 1));
            Answer a6 = new(test2, qq4, c23, new DateTime(2021, 4, 1));
            Answer a7 = new(test2, qq2, c18, new DateTime(2021, 4, 1));
            Answer a8 = new(test2, qq1, c11, new DateTime(2021, 4, 2));
            Answer a9 = new(test3, qq5, c23, new DateTime(2021, 5, 4));
            Answers.AddRange(new[] { a1, a2, a3, a4, a5, a6, a7, a8, a9 });
            SaveChanges();

            Database.CommitTransaction();
        }

    }
}
