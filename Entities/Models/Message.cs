using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public ApplicationUser User { get; set; }
        public Section Section { get; set; }    
    }
}
