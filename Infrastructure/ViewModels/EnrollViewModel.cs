using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class EnrollViewModel
    {
        public int SelectedSubjectId { get; set; }
        public string SelectedSectionID { get; set; }
        public bool isEnrolled { get; set; }
    }
}
