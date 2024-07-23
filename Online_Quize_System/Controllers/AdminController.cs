using Entities.Models;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;
using System.Data;

namespace Online_Quize_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ISubjectService _subjectService;
        private readonly ISectionService _sectionService;
        private readonly IQuizService _quizService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService _adminService, ISubjectService subjectService, 
            ISectionService sectionService, IQuizService quizService)
        {
            this._adminService = _adminService;
            _subjectService = subjectService;
            _sectionService = sectionService;
            _quizService = quizService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _adminService.GetAllUsers();
            ViewBag.UsersNumbers = users.Count();
            var subjects = await _subjectService.GetAllSubjects();
            ViewBag.subjects = subjects.Count();
            return View();
        }
        public async Task<IActionResult> AddSubject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _adminService.AddSubject(model);
                if (Response.IsDone)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }

        public async Task<IActionResult> AddSection()
        {
            var subjects = await _subjectService.GetAllSubjects();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSection(SectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _adminService.AddSection(model);
                if (Response.IsDone)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }
        [Authorize(Roles = "Admin")]

        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel role)
        {
            var Response= await _adminService.AddRole(role);
            if (Response.IsDone)
            {
                return Content("Created");
            }
            return Content("NotFound");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole()
        {
            var students = await _adminService.GetAllUsers();
            ViewBag.students = new SelectList(students, "Id", "Name");
            var roles = await _adminService.GetAllRoles();
            ViewBag.roles = new SelectList(roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {
            var Response = await _adminService.AddRoleRoUser(model);
            if (Response.IsDone)
            {
                return Content("Added To Role");
            }
            return Content("Wrong");
        }
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _adminService.AddUser(model);
                if(Response.IsDone)
                {
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("", "Invalid Email or User Name try Again");
                return RedirectToAction("AddUser",model);
            }
            return View(model);
        }
        public  async Task<IActionResult> DeleteUser()
        {
            var users = await _adminService.GetAllUsers();
           // ViewData["users"] = new SelectList(users, "Id", "Name");
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if(id != null)
            {
                var Response = await _adminService.DeleteUser(id);
                if (Response.IsDone)
                {
                    return RedirectToAction("Index");
                }
                return View(Response.Message);
            }
            ModelState.AddModelError("", "Please Select User To Delete It");
            return View();
        }
        [Authorize]
        public async Task<IActionResult> UsersEvaluations()
        {
            var subjects = await _subjectService.GetAllSubjects();
            ViewBag.subjects = new SelectList(subjects, "Id", "Name");
            var sections = await _sectionService.GetAllSections();
            ViewBag.sections = new SelectList(sections, "Id", "Name");
            var quizzes = await _quizService.GetAllQuizzes();
            ViewBag.quizzes = new SelectList(quizzes, "Id", "Name");
            return View("UsersEvaluations");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Filtrations(FilterUsersEvaluationViewModel model)
        {
            var response = await _adminService.Filtrations(model);
            if (response != null && response.Any())
            {
                return Json(response); 
            }
            return Json(new List<UsersEvaluationViewModel>());
        }


    }
}
