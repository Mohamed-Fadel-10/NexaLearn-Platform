using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class FilterUsersEvaluationViewModel
    {
        public int? QuizId { get; set; }
        public int? SectionId { get; set; }
        public int? SubjectId { get; set; }

    }
}
