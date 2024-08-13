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
        public string? Id { get; set; }

        [Required]
        public string? Name { get; set; }
        [MaxLength(15,ErrorMessage ="Max Length for UserName : 15 Char ")]
        [Required]
        public string? UserName { get; set; }
        public IFormFile? Photo { get; set; }
        [MaxLength(11, ErrorMessage = "Max Length for Phone 11 Digit")]
        [MinLength(11, ErrorMessage = "Min Length for Phone 11 Digit")]
        public string? Phone { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Email Address Not Valid!")]
        public string? Email { get; set; }
        public string? Major { get; set; }
        public string? PhotoPath { get; set; } 


    }
}
