using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class QuizViewModel
    {
        public int QuizID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? SessionID { get; set; }
        public double TotalDegree { get; set; }
        public double PassingDegree { get; set; }
        public int? QuestionNumber { get; set;  }
        public string? CreatorName { get; set; }
        public int SubjectId { get; set; }  
        public bool IsPrivate { get; set; } = false;
       public TimeSpan? Duration { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public List<QuestionViewModel>? Questions { get; set; } = new List<QuestionViewModel>();

    }
}
