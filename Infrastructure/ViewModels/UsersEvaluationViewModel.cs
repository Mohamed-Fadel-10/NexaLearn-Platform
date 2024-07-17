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
        public int QuizID { get; set; }
        public int QuestionID { get; set; }
        public string? Answer { get; set; }
        public double? Score { get; set; }
    }
}
