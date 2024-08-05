using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class UserProfileDataViewModel
    {
        public string? Name { get; set; }
        [MaxLength(15,ErrorMessage ="Max Length for UserName : 15 Char ")]
        public string? UserName { get; set; }
        public IFormFile? Photo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Major { get; set; }
        public string? PhotoPath { get; set; } 


    }
}
