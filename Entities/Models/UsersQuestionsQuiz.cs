using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UsersQuestionsQuiz
    {
        public int Id { get; set; } 

        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Quiz")]
        public int QuizID  { get; set; }
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        [ForeignKey("Section")]
        public int SectionID { get; set; }
        public virtual Question Question { get; set; }
        public virtual Section Section { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Quiz Quiz { get; set; }
        public string? Answer { get; set; }
        public double? Score { get; set; }
        public DateTime? SubmissionDate { get; set; }


    }
}
