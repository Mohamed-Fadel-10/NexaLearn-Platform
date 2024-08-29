using Entities.Models;
using Infrastructure.Hubs;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using Services.Interfaces;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace Online_Quize_System.Controllers
{
    public class UsersController : Controller
    {
        private readonly IStudentService _usersService;
        private readonly IHubContext<QuizHub> _hubContext;
        private readonly ISubjectService _subjectService;
        private readonly ISectionService _sectionService;
        public UsersController(IStudentService _usersService, IHubContext<QuizHub> _hubContext,ISubjectService _subjectService,ISectionService _sectionService) {
            this._usersService = _usersService;
            this._hubContext= _hubContext;
            this._subjectService= _subjectService;
            this._sectionService= _sectionService;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var response = await _usersService.GetAll();
            return View(response);
        }
        [Authorize]
        public async Task<IActionResult> Sections()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _sectionService.StudentSections(userId);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Evaluate(string answers, int SectionID)
        {
            if (string.IsNullOrEmpty(answers))
            {
                return Content("Not Valid Model");
            }

            var model = JsonConvert.DeserializeObject<List<UsersEvaluationViewModel>>(answers);

            if (model == null || !model.Any())
            {
                return Content("Not Valid Model");
            }

            var response = await _usersService.Evaluate(model, SectionID);

            if (response != null)
            {
                await _hubContext.Clients.All
                      .SendAsync("ReceiveNotification", response.QuizID, response.QuizName, response.QuizSession,
                      response.UserName, response.Score,
                      DateTime.UtcNow.ToString("o"), response.Subject, response.Section);

                return View("QuizResult", response);
            }
            return Content("There Is a Problem Occurred, Please Try Again");
        }


        [Authorize]
        public async Task<IActionResult> Enroll()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(string code)
        {           
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _usersService.Enroll(code, userId);
            if (response.IsDone)
            {
                return RedirectToAction("Sections");
            }
            else if (!response.IsDone && response.Message == "You Already Enrolled In This Section Before")
            {
                ViewBag.Message = "You Already Enrolled In This Section Before";
                return View("SectionMessagePage");
            }
            else
            {
                ViewBag.Message = "Section Not Found !";
                return View("SectionMessagePage");
            }

        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
           var response= await _usersService.GetUser(id);          
            return View(response);
        }

    }
}
