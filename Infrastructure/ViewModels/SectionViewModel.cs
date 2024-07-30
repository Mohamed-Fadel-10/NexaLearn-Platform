using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        [Required]
        [MaxLength(5,ErrorMessage ="Address Must Be Less Than 5 Char")]
        public string? Address { get; set; }
        public int SubjectId { get; set; }
        public virtual ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

    }
}
