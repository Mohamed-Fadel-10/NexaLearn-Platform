using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Online_Quize_System.Controllers
{
    public class AnswersController : Controller
    {
        private readonly IQuestionsService _questionsService;

        public AnswersController(IQuestionsService questionsService)
        {
            _questionsService = questionsService;
        }
        public async Task< IActionResult> ViewAnswers(string userId,int quizId)
        {
            var response=await _questionsService.StudentsQuestionsAnswers(userId, quizId);
            if (response != null)
            {
                return View(response);
            }
            return View(null);
        }
    }
}
