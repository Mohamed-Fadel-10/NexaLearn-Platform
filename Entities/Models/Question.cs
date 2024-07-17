using Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Points { get; set; }
        public QuestionType QuestionType { get; set; }
        public string? Hint { get; set; }
        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<MultipleChoice> Options { get; set; } = new List<MultipleChoice>();
        public virtual ICollection<UsersQuestionsQuiz> UsersEvaluations { get; set; } = new List<UsersQuestionsQuiz>();


    }
}
