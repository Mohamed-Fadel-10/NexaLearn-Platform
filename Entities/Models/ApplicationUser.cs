using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string? Major { get; set; }
        public string? Photo { get; set; }
        public virtual ICollection<Quiz>?CreatedQuizzes { get; set; }
        public virtual ICollection<UsersQuestionsQuiz> UsersEvaluations { get; set; } = new List<UsersQuestionsQuiz>();
        public virtual ICollection<StudentsSections> StudentsSections { get; set; }= new List<StudentsSections>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
