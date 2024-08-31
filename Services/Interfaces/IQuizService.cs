using Entities.Models;
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
        public Task<QuizViewModel> GetQuiz(string SessionID);
        public Task<IEnumerable<Quiz>> GetAllQuizzes();
        public Task<Response> GetById(int id);
        public Task<QuizViewModel> GetQuizQuestionsById(int id);
        public List<OptionsViewModel> GetOptionsByQuestionId(int questionId);
        public Task<Response> UpdateQuiz(QuizViewModel model, int id);
        public Task<Response> DeleteQuiz(int id);
        public Task<QuizViewModel> Details(int id);
        public Task<Response> MakePrivate(int id, bool isPrivate);
        public Task<Response> MakeEnabled(int id, bool IsEnabled);
        public Task<bool> IsOpened(string UserId, int QuizID);
        public Task<bool> IsQuizForStudentSections(string userId, string sessionID);
    }
}
