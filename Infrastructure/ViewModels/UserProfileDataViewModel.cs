using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class UserProfileDataViewModel
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Major { get; set; }
        public string? PhotoPath { get; set; } 


    }
}
