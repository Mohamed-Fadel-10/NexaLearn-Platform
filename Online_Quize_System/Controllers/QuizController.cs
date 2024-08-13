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
        private readonly ISubjectService _subjectService;

        public QuizController(IQuizService _quizService, QuizContext _context, IAdminService adminService, ISubjectService subjectService)
        {
            this._quizService = _quizService;
            this._context = _context;
            _adminService = adminService;
            _subjectService = subjectService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddQuiz()
        {
            var model = new QuizViewModel();
            var subjects = await _subjectService.GetAllSubjects();
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

        [Authorize(Roles = "Admin")]
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
                        CreatedOn=quiz.CreatedOn,
                        Duration=quiz.Duration,
                        Description=quiz.Description,
                        TotalDegree=quiz.TotalDegree,
                        PassingDegree= (double)quiz.PassingScore,
                        IsPrivate=quiz.IsPrivate,
                        Name=quiz.Name
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
        public async Task<IActionResult> StartQuiz(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                ModelState.AddModelError("", "Session ID is required to open the quiz.");
                return View("GetQuizBySession");
            }

            sessionId = sessionId.Trim();
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var quiz = await _quizService.GetQuiz(sessionId);
            if (quiz == null || quiz.CreatedOn == null)
            {
                return RedirectToAction("NotFoundQuiz");
            }

            var isQuizInEnrolledSections = await _quizService.IsQuizForStudentSections(userId, sessionId);
            if (!isQuizInEnrolledSections)
            {
                ViewBag.Msg = "You are not enrolled in a section for this quiz.";
                return View("QuizAccessDenied");
            }

            var hasAlreadyOpened = await _quizService.IsOpened(userId, quiz.QuizID);
            if (hasAlreadyOpened)
            {
                ViewBag.Msg = "You have already completed this quiz. You cannot enter it again.";
                return View("QuizAccessDenied");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            ViewBag.UserName = user?.UserName;
            ViewBag.UserId = userId;

            return View("StartQuiz", quiz);
        }


        public IActionResult NotFoundQuiz()
        {
            return View("NotFoundQuiz");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var Response = await _quizService.GetAllQuizzes();
            if (Response.Any())
            {
                return View("ListOfQuizzes", Response);
            }
            return NotFound();

        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var Response = await _quizService.DeleteQuiz(id);
            if (Response.IsDone)
            {
                return RedirectToAction("GetAllQuizzes");
            }
            return NotFound();

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var Response = await _quizService.Details(id);
            if (Response!=null)
            {
                return View(Response);
            }
            return View("NotFoundPage");

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> TogglePrivate(int id,bool isPrivate)
        {
            var Response = await _quizService.MakePrivate(id, isPrivate);
            if (Response.IsDone)
            {
                return Content("Done");
            }
            return Content("False");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ToggleEnabled(int id, bool isEnabled)
        {
            var Response = await _quizService.MakeEnabled(id, isEnabled);
            if (Response.IsDone)
            {
                return Content("Done");
            }
            return Content("False");
        }
    }
}
