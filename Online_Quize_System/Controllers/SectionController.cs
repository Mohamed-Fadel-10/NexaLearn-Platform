using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Services.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Online_Quiz_System.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SectionController(ISectionService sectionService, IWebHostEnvironment webHostEnvironment)
        {
            _sectionService = sectionService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

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

        // Action method to download a file
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
    }
}
