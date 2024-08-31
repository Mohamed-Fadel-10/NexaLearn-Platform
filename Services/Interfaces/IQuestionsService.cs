using Infrastructure.Response;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IQuestionsService
    {
        public  Task<Response> AddQuestions(List<QuestionViewModel> model, int quizId);
        public Task<IEnumerable<StudentsAnswersViewModel>> StudentsQuestionsAnswers(string userId, int quizID);
    }
}
