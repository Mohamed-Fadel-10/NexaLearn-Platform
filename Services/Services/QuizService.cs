using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class QuizService : IQuizService
    {
        private QuizContext _context;
        public QuizService(QuizContext context)
        {
            _context = context;
        }
        public async Task<Response> AddQuiz(QuizViewModel model,string CreatorId)
        {
            if (model != null)
            {
                var quiz = new Quiz();
                var question = new Question();
                quiz.Name = model.Name;
                quiz.CreatedOn=DateTime.Now;
                quiz.Description = model.Description;
                quiz.TotalDegree = model.TotalDegree;
                quiz.QuestionNumbers = model.QuestionNumber;
                quiz.PassingScore = model.PassingDegree;
                quiz.IsPrivate = model.IsPrivate;
                quiz.SubjectId = model.SubjectId;
                quiz.CreatorId = CreatorId;
                quiz.SessionID= Guid.NewGuid().ToString();
                quiz.Duration = model.Duration;
                await _context.Quiz.AddAsync(quiz);
                await _context.SaveChangesAsync();              
                return new Response { IsDone = true, Model = quiz.Id };
            }
            return new Response { IsDone = false, Model = null };

        }
        public async Task<Response> AddQuestions(List<QuestionViewModel> model, int QuizId)
        {
            if (model != null)
            {
                foreach(var question in model)
                {
                    var Question = new Question
                    {
                        Title= question.QuestionText,
                        Points=(double)question.Points,
                        Hint=question.Hint,
                        QuizId=QuizId,
                        QuestionType= (QuestionType)question.Type
                    };
                    await _context.Question.AddAsync(Question);
                    await _context.SaveChangesAsync();

                    if (question.Type ==(int) QuestionType.MultipleChoice)
                    {
                        foreach (var option in question.Options)
                        {
                            var multipleChoice = new MultipleChoice
                            {
                                CorrectAnswer = question.CorrectAnswer,
                                IsCorrect = option.IsCorrect,
                                QuestionId = Question.Id,
                                Option=option.Text                             
                            };
                            await _context.MultipleChoices.AddAsync(multipleChoice);
                        }
                    }
                    else if(question.Type == (int)QuestionType.TrueFalse)
                    {
                        var trueFalse = new TrueFalse
                        {
                            CorrectAnswer = question.CorrectAnswer,
                            QuestionId = Question.Id
                        };
                        await _context.TrueFalse.AddAsync(trueFalse);

                    }
                    else
                    {
                        var shortText = new ShortText
                        {
                            CorrectAnswer = question.CorrectAnswer,
                            QuestionId = Question.Id
                        };
                        await _context.ShortText.AddAsync(shortText);
                    }
                }
                await _context.SaveChangesAsync();
                return new Response() { IsDone = true};
            }
            return new Response() { IsDone = false };
            }
        public async Task<QuizViewModel> GetQuiz(string SessionID)
        {
            var quiz = await _context.Quiz
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.SessionID == SessionID);
            if (quiz != null)
            {
                var QuizWithQuestions = new QuizViewModel()
                {   QuizID=quiz.Id,
                    Name= quiz.Name,
                    Description= quiz.Description,
                    Duration= quiz.Duration,
                    TotalDegree= quiz.TotalDegree,
                    Questions= quiz.Questions
                    .Select( s=>new QuestionViewModel
                    {
                        Id=s.Id,
                        QuestionText= s.Title,
                        Hint= s.Hint,
                        Points=s.Points,
                        Type=(int)s.QuestionType,
                        Options = s.Options.Select(s=>new 
                        OptionsViewModel { 
                            Text=s.Option
                        }).ToList()

                    }).ToList()
                };
                

                return QuizWithQuestions;
            }
            return new QuizViewModel();
        }
        public async Task<Response> GetAllQuizzes()
        {
            var quizzes = await _context.Quiz.Include(q => q.Questions).AsNoTracking().ToListAsync();
            if (quizzes.Any())
            {
                return new Response { IsDone = true, Model = quizzes };
            }

            return new Response { IsDone = false, Model = null };
        }
        public async Task<Response> GetById(int id)
        {
            var quiz = await _context.Quiz.FirstOrDefaultAsync(q=>q.Id==id);
            if (quiz !=null)
            {
                return new Response { IsDone = true, Model = quiz };
            }

            return new Response { IsDone = false, Model = null, Message="Quiz Not Found" };
        }
        public async Task<Response> UpdateQuiz(QuizViewModel model,int id)
        {
           var quiz= await _context.Quiz.FirstOrDefaultAsync(q => q.Id == id);
            if (quiz != null)
            {
                quiz.Name = model.Name;
                quiz.Description = model.Description;
                quiz.TotalDegree = model.TotalDegree;
                quiz.PassingScore = model.PassingDegree;
                quiz.IsPrivate = model.IsPrivate;
                quiz.Duration = model.Duration;
                quiz.CreatedOn = model.CreatedOn;
                quiz.SessionID = Guid.NewGuid().ToString();            
                await _context.SaveChangesAsync();
                return new Response { IsDone=true, Model = quiz };
            }
            return new Response { IsDone = false, Model = null };
        }
        public async Task<Response> DeleteQuiz( int id)
        {
            var quiz = await _context.Quiz.FirstOrDefaultAsync(q => q.Id == id);
            if (quiz != null)
            {
                var questions = _context.Question.Where(q => q.QuizId == id).ToList();
                _context.Question.RemoveRange(questions);
                _context.Quiz.Remove(quiz);
                await _context.SaveChangesAsync();
                return new Response { IsDone = true, Message="Quiz Deleted Successfully" };

            }
            return new Response { IsDone = false, Model = null};
        }
        public async Task<QuizViewModel> Details(int id)
        {
            var quiz = await _context.Quiz.FirstOrDefaultAsync(q => q.Id == id);
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Id == quiz.CreatorId);
            if (quiz != null)
            {
                return new QuizViewModel
                {
                    Name = quiz.Name,
                    CreatorName=User.UserName,
                    Description=quiz.Description,
                    Duration=quiz.Duration,
                    IsPrivate=quiz.IsPrivate,
                    QuestionNumber=quiz.QuestionNumbers,
                    TotalDegree=quiz.TotalDegree,
                    PassingDegree=(double)quiz.PassingScore,
                    SessionID=quiz.SessionID,
                    CreatedOn=(DateTime)quiz.CreatedOn
                };
            }

            return new QuizViewModel();
        }
        public async Task<Response> MakePrivate(int id, bool isPrivate)
        {
            var Quiz =await _context.Quiz.FirstOrDefaultAsync(q => q.Id == id);
            if(Quiz!= null)
            {
                Quiz.IsPrivate=isPrivate;
                await _context.SaveChangesAsync();
                return new Response { IsDone = true };
            }
            return new Response { IsDone = false };
        }
        public async Task<Response> MakeEnabled(int id, bool IsEnabled)
        {
            var Quiz = await _context.Quiz.FirstOrDefaultAsync(q => q.Id == id);
            if (Quiz != null)
            {
                Quiz.IsEnabled = IsEnabled;
                await _context.SaveChangesAsync();
                return new Response { IsDone = true };
            }
            return new Response { IsDone = false };
        }
    }
}
