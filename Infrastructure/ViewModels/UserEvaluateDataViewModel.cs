using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class UserEvaluateDataViewModel
    {
        public string Name { get; set; }
        public string QuizName { get; set; }
        public string UserId { get; set; }
        public int QuizId { get; set; }
        public DateTime SubmitDate { get; set;}

    }
}
