using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Repsitories;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Unit_Of_Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class QuestionsService : GenericRepository<Quiz>, IQuestionsService
    {
        private IUnitOfWork _unitOfWork;
        private IQuizService _quizService;
        public QuestionsService(QuizContext context, IUnitOfWork unitOfWork, IQuizService quizService) : base(context)
        {
            _unitOfWork = unitOfWork;
            _quizService = quizService;
        }
        public async Task<Response> AddQuestions(List<QuestionViewModel> model, int quizId)
        {
            if (model == null)
            {
                return new Response() { IsDone = false };
            }

            foreach (var question in model)
            {
                var questionEntity = await CreateQuestionAsync(question, quizId);
                if (questionEntity == null)
                {
                    return new Response() { IsDone = false };
                }

                await AddQuestionDetailsAsync(question, questionEntity.Id);
            }

            await _context.SaveChangesAsync();
            return new Response() { IsDone = true };
        }

        private async Task<Question> CreateQuestionAsync(QuestionViewModel question, int quizId)
        {
            var questionEntity = new Question
            {
                Title = question.QuestionText,
                Points = (double)question.Points,
                Hint = question.Hint,
                QuizId = quizId,
                QuestionType = (QuestionType)question.Type
            };

            await _unitOfWork.Questions.AddAsync(questionEntity);
            await _unitOfWork.SaveAsync();
            return questionEntity;
        }

        private async Task AddQuestionDetailsAsync(QuestionViewModel question, int questionId)
        {
            switch ((QuestionType)question.Type)
            {
                case QuestionType.MultipleChoice:
                    await AddMultipleChoiceOptionsAsync(question, questionId);
                    break;

                case QuestionType.TrueFalse:
                    await AddTrueFalseQuestionAsync(question, questionId);
                    break;

                case QuestionType.ShortText:
                    await AddShortTextQuestionAsync(question, questionId);
                    break;

                default:
                    throw new ArgumentException("Unsupported question type");
            }
        }

        private async Task AddMultipleChoiceOptionsAsync(QuestionViewModel question, int questionId)
        {
            foreach (var option in question.Options)
            {
                var multipleChoice = new MultipleChoice
                {
                    CorrectAnswer = question.CorrectAnswer,
                    IsCorrect = option.IsCorrect,
                    QuestionId = questionId,
                    Option = option.Text
                };
                await _context.MultipleChoices.AddAsync(multipleChoice);
            }
        }

        private async Task AddTrueFalseQuestionAsync(QuestionViewModel question, int questionId)
        {
            var trueFalse = new TrueFalse
            {
                CorrectAnswer = question.CorrectAnswer,
                QuestionId = questionId
            };
            await _context.TrueFalse.AddAsync(trueFalse);
        }

        private async Task AddShortTextQuestionAsync(QuestionViewModel question, int questionId)
        {
            var shortText = new ShortText
            {
                CorrectAnswer = question.CorrectAnswer,
                QuestionId = questionId
            };
            await _context.ShortText.AddAsync(shortText);
        }

        public IEnumerable<StudentsAnswersViewModel> StudentsQuestionsAnswers(string userId, int quizID)
        {
            var studentsAnswers = new List<StudentsAnswersViewModel>();

            var questions = _context.UsersQuestionsQuizzes
                .Join(_context.Question,
                    uqq => uqq.QuestionID,
                    q => q.Id,
                    (uqq, q) => new { UsersQuestionsQuizzes = uqq, Question = q })
                .AsNoTracking()
                .Where(uqq => uqq.UsersQuestionsQuizzes.UserId == userId && uqq.UsersQuestionsQuizzes.QuizID == quizID)
                .GroupBy(q => q.Question.Id) 
                .ToList();

            foreach (var questionGroup in questions)
            {
                var question = questionGroup.First();
                if (question.Question.QuestionType == QuestionType.MultipleChoice)
                {
                    studentsAnswers.AddRange(GetMultipleChoiceAnswers(userId, quizID, question.Question.Id));
                }
                else if (question.Question.QuestionType == QuestionType.TrueFalse)
                {
                    studentsAnswers.AddRange(GetTrueFalseAnswers(userId, quizID, question.Question.Id));
                }
                else
                {
                    studentsAnswers.AddRange(GetShortTextAnswers(userId, quizID, question.Question.Id));
                }
            }

            return studentsAnswers;
        }

        private IEnumerable<StudentsAnswersViewModel> GetMultipleChoiceAnswers(string userId, int quizID, int questionId)
        {
            return _context.UsersQuestionsQuizzes
                .Join(_context.MultipleChoices,
                    qu => qu.QuestionID,
                    mc => mc.QuestionId,
                    (qu, mc) => new { Question = qu, MultipleChoices = mc })
                .AsNoTracking()
                .Where(q => q.Question.UserId == userId && q.Question.QuizID == quizID && q.Question.QuestionID == questionId)
                .GroupBy(q => q.Question.QuestionID) 
                .Select(g => new StudentsAnswersViewModel
                {
                    QuestionText = g.First().Question.Question.Title,
                    Points = (double)g.First().Question.Question.Points,
                    Score = (double)g.First().Question.Score,
                    StudentAnswer = g.First().Question.Answer,
                    Options = g.Select(q => q.MultipleChoices).Distinct().Select(mc => new OptionsViewModel
                    {
                        IsCorrect = mc.IsCorrect,
                        Text = mc.Option
                    }).ToList()
                })
                .ToList();
        }


        private IEnumerable<StudentsAnswersViewModel> GetTrueFalseAnswers(string userId, int quizID, int questionId)
        {
            return _context.UsersQuestionsQuizzes
                .Join(_context.TrueFalse,
                    qu => qu.QuestionID,
                    tf => tf.QuestionId,
                    (qu, tf) => new { Question = qu, TrueFalse = tf })
                .AsNoTracking()
                .Where(q => q.Question.UserId == userId && q.Question.QuizID == quizID && q.Question.QuestionID == questionId)
                .GroupBy(q => q.Question.QuestionID) 
                .Select(g => new StudentsAnswersViewModel
                {
                    QuestionText = g.First().Question.Question.Title,
                    Points = (double)g.First().Question.Question.Points,
                    Score = (double)g.First().Question.Score,
                    StudentAnswer = g.First().Question.Answer,
                    CorrectAnswer = g.First().TrueFalse.CorrectAnswer,
                })
                .ToList();
        }

        private IEnumerable<StudentsAnswersViewModel> GetShortTextAnswers(string userId, int quizID, int questionId)
        {
            return _context.UsersQuestionsQuizzes
                .Join(_context.ShortText,
                    qu => qu.QuestionID,
                    st => st.QuestionId,
                    (qu, st) => new { Question = qu, ShortText = st })
                 .AsNoTracking()
                .Where(q => q.Question.UserId == userId && q.Question.QuizID == quizID && q.Question.QuestionID == questionId)
                .Select(s => new StudentsAnswersViewModel
                {
                    QuestionText = s.Question.Question.Title,
                    Points = (double)s.Question.Question.Points,
                    Score = (double)s.Question.Score,
                    StudentAnswer = s.Question.Answer,
                    CorrectAnswer = s.ShortText.CorrectAnswer,
                }).ToList();
        }


    }
}
