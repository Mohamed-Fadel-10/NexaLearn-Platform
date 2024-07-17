﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxDegree { get; set; }
        public int? MinDegree { get; set; }
        public virtual ICollection<Section> Sections  { get; set; }= new List<Section>();
    }
}
