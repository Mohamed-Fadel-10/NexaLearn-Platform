﻿using Entities.Models;
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
        private readonly IMaterialsService _materialsService;

        public AdminController(IAdminService _adminService, ISubjectService _subjectService, 
            ISectionService _sectionService, IQuizService _quizService, IMaterialsService _materialsService)
        {
            this._adminService = _adminService;
            this._subjectService = _subjectService;
            this._sectionService = _sectionService;
            this._quizService = _quizService; 
            this._materialsService = _materialsService;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _adminService.GetAllUsers();
            ViewBag.UsersNumbers = users.Count();
            var subjects = await _subjectService.GetAllSubjects();
            ViewBag.subjects = subjects.Count();
            var sections=await  _sectionService.GetAllSections();
            ViewBag.Sections= sections.Count();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(Subject model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _subjectService.AddSubject(model);
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            
                var Response = await _adminService.DeleteUser(id);
                if (Response)
                {
                    return RedirectToAction("Index","Users");
                }
                return Content("Error");
         
        }
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filtrations(FilterUsersEvaluationViewModel model)
        {
            var response = await _adminService.Filtrations(model);
            if (response != null && response.Any())
            {
                return Json(response); 
            }
            return Json(new List<UsersEvaluationViewModel>());
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddMaterials()
        {
            var subjects = await _subjectService.GetAllSubjects();
            ViewBag.subjects = new SelectList(subjects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMaterials(AddMaterialsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _materialsService.AddMaterials(model);
                if (response.IsDone)
                {               
                    return RedirectToAction("Index");
                }
                return View();
            }
            ModelState.AddModelError("", "Model Data Not Valid");
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetSections(int subjectId)
        {
            var sections = await _sectionService.SectionsBySubjectID(subjectId);

            var sectionList = sections.Select(s => new {
                value = s.Id,  
                text = s.Name  
            }).ToList();

            return Json(sectionList);
        }



    }
}
