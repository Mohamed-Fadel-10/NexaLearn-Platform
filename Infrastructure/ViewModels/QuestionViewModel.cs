using Entities.Models;
using Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
         public int? Type { get; set; }
         public string QuestionText { get; set; }
         public string? Hint { get; set; } 
         public double? Points { get; set; }
         public int QuizId { get; set; }
        /// /////////////   ANSWERS   //////////////////////////////
        public bool? IsCorrect { get; set; }
        public List<OptionsViewModel> Options { get; set; } = new List<OptionsViewModel>();
        public string? CorrectAnswer { get; set; }
    }
}
