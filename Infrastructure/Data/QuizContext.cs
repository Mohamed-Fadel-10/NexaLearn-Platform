using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class QuizContext:IdentityDbContext<ApplicationUser>
    {
        public QuizContext (DbContextOptions<QuizContext> options):base(options) { }
        public QuizContext()  { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersQuestionsQuiz>()
                .HasKey(uqq => uqq.Id);

            modelBuilder.Entity<UsersQuestionsQuiz>()
                .HasIndex(uqq => new { uqq.QuizID, uqq.QuestionID, uqq.UserId })
                .IsUnique(false);

            modelBuilder.Entity<UsersQuestionsQuiz>()
                .HasOne(uqq => uqq.Quiz)
                .WithMany(q => q.UsersEvaluations)
                .HasForeignKey(uqq => uqq.QuizID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsersQuestionsQuiz>()
                .HasOne(uqq => uqq.Question)
                .WithMany(q => q.UsersEvaluations)
                .HasForeignKey(uqq => uqq.QuestionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsersQuestionsQuiz>()
                .HasOne(uqq => uqq.User)
                .WithMany(u => u.UsersEvaluations)
                .HasForeignKey(uqq => uqq.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentsSections>()
                .HasKey(uqq => new { uqq.SectionId,uqq.UserId });

            

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Quiz> Quiz { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Question> Question  { get; set; }
        public virtual DbSet<FeedBack> FeedBack { get; set; }
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<UsersQuizFeedbacks> UsersQuizFeedbacks { get; set; }
        public virtual DbSet<MultipleChoice> MultipleChoices { get; set; }
        public virtual DbSet<TrueFalse> TrueFalse { get; set; }
        public virtual DbSet<ShortText> ShortText { get; set; }
        public virtual DbSet<UsersQuestionsQuiz> UsersQuestionsQuizzes { get; set; }
        public virtual DbSet<StudentsSections> StudentsSections { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<OpenedQuizzes> OpenedQuizzes { get; set; }










    }
}
