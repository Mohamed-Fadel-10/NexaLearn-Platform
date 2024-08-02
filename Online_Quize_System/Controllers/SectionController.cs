using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Services.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Services;

namespace Online_Quiz_System.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly ISubjectService _subjectService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionController(ISectionService sectionService, IWebHostEnvironment webHostEnvironment, ISubjectService subjectService)
        {
            _sectionService = sectionService;
            _webHostEnvironment = webHostEnvironment;
            _subjectService = subjectService;
        }

        public IActionResult Index()
        {
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
                var Response = await _sectionService.AddSection(model);
                if (Response.IsDone)
                {
                    return RedirectToAction("SectionsWithStudents", "Section");
                }
                return View();
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSectionDetails(int sectionId)
        {
            var sectionDetails = await _sectionService.GetSectionDetails(sectionId);
            if (sectionDetails.Any())
            {
                return Json(sectionDetails.First());
            }
            return Content("Not Found");
        }

        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        [HttpGet]
        public IActionResult ViewFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest("Filename cannot be null or empty.");
            }

            string uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            string fullName = Path.Combine(uploadsPath, filename);

            if (!System.IO.File.Exists(fullName))
            {
                return NotFound("File not found.");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
            return File(fileBytes, "application/pdf");
        }
        public async Task<IActionResult> SectionsWithStudents()
        {
            ViewBag.sections = new SelectList(await _sectionService.GetAllSections(), "Id", "Name");
            var data = await _sectionService.SectionsWithStudentsNumbers(null);
            return View(data);
        }

        public async Task<IActionResult> GetSectionData(int sectionId)
        {
            var data = await _sectionService.SectionsWithStudentsNumbers(sectionId);
            return Json(data);
        }

        public async Task<IActionResult> StudentsSection()
        {
            ViewBag.sections = new SelectList(await _sectionService.GetAllSections(), "Id", "Name");
            var data = await _sectionService.StudentsInSection(null);
            return View(data);
        }

        public async Task<IActionResult> StudentsInSection(int sectionId)
        {
            var data = await _sectionService.StudentsInSection(sectionId);
            return Json(data);
        }


    }
}
