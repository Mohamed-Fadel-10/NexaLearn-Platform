using Infrastructure.Response;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IQuizService
    {
        public Task<Response> AddQuiz(QuizViewModel model, string CreatorId);
        public Task<Response> AddQuestions(List<QuestionViewModel> model, int QuizId);
        public Task<QuizViewModel> GetQuiz(string SessionID);
        public Task<Response> GetAllQuizzes();
        public Task<Response> GetById(int id);
        public Task<Response> UpdateQuiz(QuizViewModel model, int id);
        public Task<Response> DeleteQuiz(int id);
        public Task<QuizViewModel> Details(int id);
    }
}
