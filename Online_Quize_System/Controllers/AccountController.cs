﻿using Entities.Models;
using Humanizer;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Services.Interfaces;
using Services.Services;
using System.Security.Claims;

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
        public async Task<IActionResult> LogIn(LogInViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.LogInAsync(model);

                if (response.IsDone)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect username or password");
            }

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
          await _accountService.LogOut();
           return RedirectToAction("Index","Home");
        }
        #region External LogIN
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
        #endregion


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.User
                   .FindFirstValue(ClaimTypes.NameIdentifier);
            var response= await _accountService.UserData(userId);
            return View(response);
        }
        public async Task<IActionResult> ProfileEdit ()
        {
            var userId = HttpContext.User
                   .FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _accountService.UserData(userId);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserProfileDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);
              var response=  await _accountService.Profile(model, userId);
                if (response.IsDone)
                {
                    return RedirectToAction("Profile"); 
                }
            }
            ModelState.AddModelError("", "Error in Data Model try Again");
            return RedirectToAction();
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _accountService.ChangePassword(model, userId);

                if (response.IsDone)
                {
                    return RedirectToAction("Profile");
                }

                ModelState.AddModelError("", "Cannot Change Password Now, Try Again");
                return View(response.Model);
            }

            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyCurrentPassword(string currentPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!await _accountService.CheckCurrentPassword(currentPassword, userId))
            {
                return Json($"Current password is incorrect.");
            }

            return Json(true);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyCurrentUserName(string userName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await _accountService.IsFoundUserName(userId, userName))
            {
                return Json($"UserName is already taken by another user.");
            }

            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyCurrentEmail(string email)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await _accountService.IsFoundEmail(userId, email))
            {
                return Json($"Email is already taken by another user.");
            }

            return Json(true);
        }

        public  IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.ForgetPassword(model);
                if (response.IsDone)
                {
                    return RedirectToAction("ForgetPasswordConfirmation");
                }
                return View("ForgetPassword");
            }
            return View("ForgetPassword");
        }

        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }

            var model = new ResetPasswordViewModel { UserId = userId, Token = token };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _accountService.ResetPassword(model);
            if (response.IsDone)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            ModelState.AddModelError("", response.Message);
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
