using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly QuizContext _context;
        public UsersService(QuizContext context)
        {
            _context = context;
        }

        public async Task<Response> Evaluate(List<UsersEvaluationViewModel> model)
        {
            if (model == null)
            {
                return new Response() { IsDone = false, Model = null };
            }

            var questionIds = model.Select(x => x.QuestionID).ToList();

            var questions = await _context.Question
                .Where(q => questionIds.Contains(q.Id))
                .ToListAsync();

            var multipleChoiceAnswers = await _context.MultipleChoices
                .Where(mc => questionIds.Contains(mc.QuestionId))
                .GroupBy(mc => mc.QuestionId)
                .Select(g => g.First())
                .ToDictionaryAsync(mc => mc.QuestionId, mc => mc.CorrectAnswer);

            var trueFalseAnswers = await _context.TrueFalse
                .Where(tf => questionIds.Contains(tf.QuestionId))
                .GroupBy(tf => tf.QuestionId)
                .Select(g => g.First())
                .ToDictionaryAsync(tf => tf.QuestionId, tf => tf.CorrectAnswer);

            var ShortTextAnswers = await _context.ShortText
                .Where(tf => questionIds.Contains(tf.QuestionId))
                .GroupBy(tf => tf.QuestionId)
                .Select(g => g.First())
                .ToDictionaryAsync(tf => tf.QuestionId, tf => tf.CorrectAnswer);

            var usersQuestionsQuizzes = new List<UsersQuestionsQuiz>();

            foreach (var item in model)
            {
                var question = questions.FirstOrDefault(q => q.Id == item.QuestionID);
                if (question != null)
                {
                    if (question.QuestionType == QuestionType.MultipleChoice)
                    {
                        if (multipleChoiceAnswers.TryGetValue(item.QuestionID, out var correctAnswer))
                        {
                            item.Score = (item.Answer == correctAnswer) ? question.Points : 0;
                        }
                    }
                    else if (question.QuestionType == QuestionType.TrueFalse)
                    {
                        if (trueFalseAnswers.TryGetValue(item.QuestionID, out var correctAnswer))
                        {
                            item.Score = (item.Answer == correctAnswer) ? question.Points : 0;
                        }
                    }
                    else
                    {
                        if(ShortTextAnswers.TryGetValue(item.QuestionID,out var correctAnswer))
                        {
                            item.Score= item.Answer == correctAnswer ? question.Points : 0;
                        }
                    }

                    usersQuestionsQuizzes.Add(
                        new UsersQuestionsQuiz
                    {
                        Answer = item.Answer,
                        UserId = item.UserId,
                        QuizID = item.QuizID,
                        QuestionID = item.QuestionID,
                        Score = item.Score
                    });
                }
            }
            await _context.UsersQuestionsQuizzes.AddRangeAsync(usersQuestionsQuizzes);
            await _context.SaveChangesAsync();

            return new Response() { IsDone = true };
        }
    }
}
