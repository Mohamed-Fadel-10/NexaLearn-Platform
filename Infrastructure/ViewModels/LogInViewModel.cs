using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage ="UserName Field Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Field Required")]
        public string Password { get; set; }


    }
}
