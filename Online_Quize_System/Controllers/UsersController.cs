using Entities.Models;
using Infrastructure.Hubs;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Services.Interfaces;

namespace Online_Quize_System.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IHubContext<QuizHub> _hubContext;
        public UsersController(IUsersService _usersService, IHubContext<QuizHub> _hubContext) {
            this._usersService = _usersService;
            this._hubContext= _hubContext;
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
                return Content("Not Valid Model");
            }

            var response = await _usersService.Evaluate(model);
            if (response != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", response.QuizID,response.QuizSession, response.UserId, response.UserName,response.Score, DateTime.UtcNow.ToString("o"));
                return RedirectToAction("Index", "Home");
            }
            return Content("There Is a Problem Occurred , Please Try again");
        }

        public IActionResult Submit()
        {
            return View();
        }
    }
}
