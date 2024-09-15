using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class UsersEvaluationViewModel
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public int QuizID { get; set; }
        public string? QuizName { get; set; }
        public int? CorrectAnswerCount { get; set; } = 0; 
        public int? QuestionsNumber { get; set; }
        public int QuestionID { get; set; }
        public string? Answer { get; set; }
        public double? Score { get; set; }
        public string? QuizSession { get; set; }
        public double? PassingDegree { get; set; }
      
        public double? TotalDegree { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string? Section { get; set; }
        public  string? Subject { get; set; }


    }
}
