using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public string Code { get; set; } 
        public string? Place { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<StudentsSections> StudentsSections { get; set; } = new List<StudentsSections>();
        public virtual ICollection<Materials> Materials { get; set; } = new List<Materials>();

    }
}
