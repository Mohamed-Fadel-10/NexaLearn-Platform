using Entities.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;

namespace Online_Quize_System.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService _usersService) {
            this._usersService = _usersService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Evaluate(string answers)
        {
            if (string.IsNullOrEmpty(answers))
            {
                return Content("not Valid Model");
            }

            var model = JsonConvert.DeserializeObject<List<UsersEvaluationViewModel>>(answers);

            if (model == null || !model.Any())
            {
                return Content("not Valid Model");
            }

            var response = await _usersService.Evaluate(model);
            if (response.IsDone)
            {
                return Content("Evaluated");
            }

            return Content("not Evaluated");
        }
        public IActionResult Submit()
        {
            return View();
        }
    }
}
