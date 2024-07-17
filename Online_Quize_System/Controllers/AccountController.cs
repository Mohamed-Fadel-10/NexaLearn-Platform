using Entities.Models;
using Humanizer;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Services.Interfaces;
using Services.Services;

namespace Online_Quize_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly IEmailService _emailService;

        public AccountController(IAccountService _accountService, UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signIn, IEmailService _emailService)
        {
            this._accountService = _accountService;
            this._userManager= _userManager;
            this._signIn= _signIn;
            this._emailService= _emailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registeration(RegisterViewModel model)
        {
            #region Old

            //if (ModelState.IsValid)
            //{
            //    var Response = await _accountService.RegisterAsync(model);
            //    if (Response.IsDone){ 
            //        return View("SendEmailConfirmation");
            //    }
            //    return RedirectToAction("Register",model);
            //}
            //return RedirectToAction("Register", model); 
            #endregion

            var user = new ApplicationUser() {
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                PasswordHash = model.Password,
                Major = "FCI"
            };
            var emailExists = await _userManager.Users.AnyAsync(u => u.Email == user.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return View("Register", model);
            }

            // Check if username already exists
            var usernameExists = await _userManager.Users.AnyAsync(u => u.UserName == user.UserName);
            if (usernameExists)
            {
                ModelState.AddModelError("UserName", "Username is already taken.");
                return View("Register",model);
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (_userManager.Options.SignIn.RequireConfirmedEmail)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmLink = Url.Action("ConfirmEmail", "Account", 
                        new { UserId = user.Id, Token = token },Request.Scheme);
                    var filePath = $"{Directory.GetCurrentDirectory()}\\Templets\\WelcomePage.html";
                    var str = new StreamReader(filePath);
                    var mailText = str.ReadToEnd();
                    str.Close();
                    mailText = mailText.Replace("[username]", model.UserName).Replace("[email]", model.Email).Replace("[confirmationLink]", confirmLink);

                    await _emailService.SendEmailAsync(user.Email, "Confirm Email", mailText);
                    return View("SendEmailConfirmation");
                }

                await _signIn.SignInAsync(user, isPersistent: false);
            }
            return View("Error");

        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }

            return View("Error");
        }

        public IActionResult LogIn(string ReturnUrl=null)
        {
            ViewData["redircurl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model,string ReturnUrl=null)
        {
            if (ModelState.IsValid)
            {
                var Response = await _accountService.LogInAsync(model);
                if (Response.IsDone)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "incorrect username or Password");
            }
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
          await _accountService.LogOut();
           return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public async Task GoogleLogIn()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims
                .Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                   
                });
            
            return RedirectToAction("Index", "Home");
        }

    }
}
