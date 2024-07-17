using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name Field Required")]
        [MaxLength(30)]
        public string Name { get; set; }
        [EmailAddress,Required(ErrorMessage = "Email Field Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "UserName Field Required"),MaxLength(15)]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password Dose not Match")]
        public string ConfirmPassword { get; set; }

    }
}
