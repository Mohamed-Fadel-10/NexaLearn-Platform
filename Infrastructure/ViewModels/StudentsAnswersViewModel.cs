using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
     public class StudentsAnswersViewModel
     {
        public string QuestionText { get; set; }
        public List<OptionsViewModel>? Options { get; set; } = new List<OptionsViewModel>();
        public double? Points { get; set; }
        public double Score { get; set; }
        public double TotalScore { get; set; }
        public string QuizName { get; set; }
        public string UserName { get; set; }
        public string StudentAnswer { get; set; }

        public string? CorrectAnswer { get; set; }


    }
}
