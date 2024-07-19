using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;
using System.Security.Claims;

namespace Online_Quize_System.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IAdminService _adminService;
        private readonly QuizContext _context;

        public QuizController(IQuizService _quizService, QuizContext _context, IAdminService adminService)
        {
            this._quizService = _quizService;
            this._context = _context;
            _adminService = adminService;
        }

        [Authorize]
        public async Task<IActionResult> AddQuiz()
        {
            var model = new QuizViewModel();
            var subjects = await _adminService.GetAllSubjects();
            ViewBag.subjects = new SelectList(subjects, "Id", "Name");
            return View(model);
            }
        [HttpPost]

        public async Task<IActionResult> AddQuiz(QuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Response = await _quizService.AddQuiz(model, userId);
                if (Response.IsDone)
                {
                    return RedirectToAction("AddQuestions", new {id =Response.Model});
                }
                    return RedirectToAction("AddQuiz", model);
            }
            return RedirectToAction("AddQuiz", model);
        }

        [Authorize]
        public IActionResult AddQuestions(int id)
        {
            var model = new List<QuestionViewModel>();
            ViewBag.Id = id;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveQuestions(List<QuestionViewModel> model, int QuizId)
        {
            if (ModelState.IsValid)
            {
                var quiz = await _context.Quiz.FirstOrDefaultAsync(s => s.Id == QuizId);
                var Response = await _quizService.AddQuestions(model, QuizId);
                if (Response.IsDone)
                {
                    return View("QuizCreatedSuccessfully",
                        new QuizViewModel { 
                        SessionID = quiz.SessionID,
                        Description=quiz.Description,
                        TotalDegree=quiz.TotalDegree,
                        PassingDegree= (double)quiz.PassingScore,
                        IsPrivate=quiz.IsPrivate,
                    });
                }
                return View("AddQuestions");
            }
            return View("AddQuestions");

          }

        [Authorize]
        public async Task<IActionResult> GetQuizBySession()
        {
            return View();
        }
      [HttpPost]
      [Authorize]
        public async Task<IActionResult> StartQuiz(string SessionID)
        {
            var Response = await _quizService.GetQuiz(SessionID.Trim());
            if (Response != null) {
                var userId = HttpContext.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.userId = userId;
                var UserName = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                ViewBag.UserName = UserName;
                return View("StartQuiz",Response);
            }
            return View("NotFoundPage");
        }
        public async Task<IActionResult> GetAllQuizzes()
        {
            var Response = await _quizService.GetAllQuizzes();
            if (Response.IsDone)
            {
                return View("ListOfQuizzes", Response.Model);
            }
            return NotFound();

        }

        public async Task<IActionResult> Edit(int id)
        {
            var Response = await _quizService.GetById(id);
            if (Response.IsDone)
            {
                return View("Edit", Response.Model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit([FromRoute]int id, QuizViewModel model)
        {
            var Response = await _quizService.UpdateQuiz(model,id);
            if (Response.IsDone)
            {
                return RedirectToAction("GetAllQuizzes");
            }
            return View("Edit",model);

        }
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var Response = await _quizService.DeleteQuiz(id);
            if (Response.IsDone)
            {
                return RedirectToAction("GetAllQuizzes");
            }
            return NotFound();

        }
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var Response = await _quizService.Details(id);
            if (Response!=null)
            {
                return View(Response);
            }
            return View("NotFoundPage");

        }

    }
}
