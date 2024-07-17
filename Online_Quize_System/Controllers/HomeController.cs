using Microsoft.AspNetCore.Mvc;

namespace Online_Quize_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
