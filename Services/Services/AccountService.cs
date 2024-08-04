using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly QuizContext _context;
        public AccountService(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signIn, QuizContext _context)
        {
            this._userManager = _userManager;
            this._signIn = _signIn;
            this._context = _context;
        }
        private async Task<ApplicationUser> GetUser(string userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(s => s.Id == userId);
            return user !=null ? user :new ApplicationUser();
        }
        public async Task<Response> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser();
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PasswordHash = model.Password;
            user.Major = "FCI";
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signIn.SignInAsync(user, isPersistent: false);
                return new Response { IsDone = true, Message = "Account Register Successfully", Model = user };
            }
            return new Response { IsDone = false, Message = "There is an problem in Registeration process" };
        }
        public async Task<Response> LogInAsync(LogInViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is not null)
            {
                SignInResult result = await _signIn.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return new Response() { IsDone = true, Message = "LogIn Process Done Successfully", Model = model };
                }
                return new Response() { IsDone = true, Message = "Invalid LogIn", Model = null! };
            }
            return new Response() { IsDone = false, Message = "User Not Found", Model = null! };
        }
        public async Task LogOut()
        {
            await _signIn.SignOutAsync();

        }
        public async Task<UserProfileDataViewModel> UserData(string userId)
        {
            var user = await GetUser(userId);

            if (user != null)
            {
                return new UserProfileDataViewModel()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Major = user.Major,
                    UserName = user.UserName,
                    PhotoPath = user.Photo != null ? $"/userImages/{user.Photo}" : null,
                    Phone = user.PhoneNumber
                };
            }
            return new UserProfileDataViewModel();
        }

        public async Task<Response> Profile(UserProfileDataViewModel model, string userId)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                if (model.Photo != null)
                {
                    var fileName = $"{Path.GetFileNameWithoutExtension(model.Photo.FileName)}_{DateTime.Now.Ticks}{Path.GetExtension(model.Photo.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userImages", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }

                    user.Photo = fileName;
                }

                if (model.Email != null && await _userManager.FindByEmailAsync(model.Email) == null)
                    {
                        user.Email = model.Email;
                    }
                    if (model.UserName != null && await _userManager.FindByNameAsync(model.UserName) == null)
                    {
                        user.UserName = model.UserName;
                    }
                    user.PhoneNumber = model.Phone;
                    user.Major = model.Major;

                    var result = await _userManager.UpdateAsync(user);
                     return new Response { IsDone = true, Model = model };
            }
            return new Response { IsDone = false, Message = "User Not Found" };
        }


        public async Task<Response> ChangePassword(ChangePasswordViewModel model,string userId)
        {
            var user = await GetUser(userId);
            if (user != null)
            {  
                var result = await _userManager
                    .ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signIn.RefreshSignInAsync(user);
                    return new Response { IsDone = true, Message = "Password Changed Successfully" };
                }
                return new Response { IsDone = false, Message = "Not Correct Password",Model=model };
            }
            return new Response { IsDone = false, Message = "User Not Found" };
        }
        public async Task<bool> CheckCurrentPassword(string currentPassword,string userId)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                var isCorrect = await _userManager.CheckPasswordAsync(user, currentPassword);
                return isCorrect ? true : false;
            }
            return false;
        }    
    }
}
