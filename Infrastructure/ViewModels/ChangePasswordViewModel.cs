using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

public class ChangePasswordViewModel
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }

    [Required]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
