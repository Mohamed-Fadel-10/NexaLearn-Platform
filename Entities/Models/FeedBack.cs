using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        [Range(1,5)]
        public int? Rate {get;set;}   
        public virtual ICollection<UsersQuizFeedbacks> UsersQuizFeedbacks { get; set; } = new List<UsersQuizFeedbacks>();
    }
}
