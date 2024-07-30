using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class SectionStudentsDataViewModel
    {
        public string SectionName { get; set; } 
        public string StudentName { get; set;}
        public string SectionCode { get; set; }
        public int StudentsNumber { get; set; }
    }
}
