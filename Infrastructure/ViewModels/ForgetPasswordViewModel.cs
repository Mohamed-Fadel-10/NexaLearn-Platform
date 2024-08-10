using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Email Address not Correct or Not Found !")]
        public string Email { get; set; }
    }
}
