using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name Field Required")]
        [MaxLength(30)]
        public string Name { get; set; }

        [EmailAddress, Required(ErrorMessage = "Email Field Required")]
        [Remote(action: "VerifyCurrentEmail", controller: "Account")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName Field Required"), MaxLength(15)]
        [Remote(action: "VerifyCurrentUserName", controller: "Account")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Field Required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{6,}$", ErrorMessage = "Password must be at least 6 characters, include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
