using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class UsersQuizFeedbacks
    {
        public int Id { get; set; } 
        public string Content { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Quiz")]
        public int  QuizId { get; set; }
        [ForeignKey("FeedBack")]
        public int FeedbackId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Quiz Quiz { get; set; }
        public virtual FeedBack FeedBack { get; set; }
    }
}
