using Entities.Models;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        public Task<Response> RegisterAsync(RegisterViewModel model);
        public Task<Response> LogInAsync(LogInViewModel model);
        public Task LogOut();


    }
}
