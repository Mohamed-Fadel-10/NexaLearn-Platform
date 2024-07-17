using Entities.Models;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService:IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signIn;
        public AccountService(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signIn) {
            this._userManager = _userManager;
            this._signIn = _signIn;
        }
        public async Task<Response> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser();
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PasswordHash = model.Password;
            user.Major = "FCI";
            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
            {
                await _signIn.SignInAsync(user, isPersistent: false);
                return new Response { IsDone = true, Message = "Account Register Successfully", Model = user };
            }
            return new Response { IsDone=false,Message="There is an problem in Registeration process"};
        }
        public async Task<Response> LogInAsync(LogInViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user is not null)
            {
                SignInResult result = await _signIn.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return new Response() { IsDone = true, Message = "LogIn Process Done Successfully", Model = model };
                }
                return new Response() { IsDone = true, Message = "Invalid LogIn", Model = null! };
            }
            return new Response() { IsDone = false, Message = "User Not Found", Model = null!};
        }
        public async Task LogOut()
        {
            await _signIn.SignOutAsync();

        }
    }
}
