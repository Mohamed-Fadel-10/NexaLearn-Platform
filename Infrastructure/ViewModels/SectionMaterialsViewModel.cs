using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
     public class SectionMaterialsViewModel
     {
        public string SectionName { get; set; }
        public List<MaterialViewModel> Materials { get; set; }

    }
}
