using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public double TotalDegree { get; set; }
        public DateTime? CreatedOn { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? QuestionNumbers { get; set; }
        public bool IsPrivate { get; set; } = true; // Private by default
        public bool IsEnabled { get; set; } = false;// Disabled by default  
        public double? PassingScore { get; set; }
        public string? SessionID { get; set; }
        [ForeignKey("Creator")]
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<Question>? Questions { get; set; }= new List<Question>();
        public virtual ICollection<UsersQuestionsQuiz> UsersEvaluations { get; set; } = new List<UsersQuestionsQuiz>();

        public virtual ICollection<UsersQuizFeedbacks>? UsersQuizFeedbacks { get; set; }= new List<UsersQuizFeedbacks>();

    }
}
