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
    public class QuizService :GenericRepository<Quiz>, IQuizService
    {
        private IUnitOfWork _unitOfWork ;
        public QuizService(QuizContext context, IUnitOfWork unitOfWork):base(context)
        {
            _unitOfWork = unitOfWork;
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
                await _unitOfWork.Quiz.AddAsync(quiz);
                await _unitOfWork.SaveAsync();              
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
                    CreatedOn=quiz.CreatedOn,
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
            return new QuizViewModel() { CreatedOn = null };
        }
        public async Task<List<Quiz>> GetAllQuizzes()
        {
            var quizzes = await _context.Quiz.Include(q => q.Questions).AsNoTracking().ToListAsync();
            if (quizzes.Any())
            {
                return quizzes;
            }

            return new List<Quiz>();
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

        public async Task<QuizViewModel> GetQuizById(int id)
        {
            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz != null)
            {
                return new QuizViewModel
                {
                    QuizID = quiz.Id,
                    Name = quiz.Name,
                    Description = quiz.Description,
                    SessionID = quiz.SessionID,
                    TotalDegree = quiz.TotalDegree,
                    PassingDegree = (double)quiz.PassingScore,
                    Duration = quiz.Duration,
                    CreatedOn = quiz.CreatedOn,
                    Questions = quiz.Questions .Select( s=>new QuestionViewModel
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
            }

            return new QuizViewModel();
        }

        public List<OptionsViewModel> GetOptionsByQuestionId(int questionId)
        {
            var options = new List<OptionsViewModel>();

            // Fetch MultipleChoice options
            var multipleChoiceOptions = _context.MultipleChoices
                .Where(mc => mc.QuestionId == questionId)
                .Select(mc => new OptionsViewModel
                {
                    Id = mc.Id,
                    Text = mc.Option,
                    IsCorrect = mc.IsCorrect ?? false
                })
                .ToList();

            options.AddRange(multipleChoiceOptions);

            // Fetch TrueFalse options
            var trueFalseOptions = _context.TrueFalse
                .Where(tf => tf.QuestionId == questionId)
                .Select(tf => new OptionsViewModel
                {
                    Id = tf.Id,
                    Text = tf.CorrectAnswer,
                    IsCorrect = true
                })
                .ToList();

            options.AddRange(trueFalseOptions);

            // Fetch ShortText options (typically, short text questions will only have one correct answer)
            var shortTextOptions = _context.ShortText
                .Where(st => st.QuestionId == questionId)
                .Select(st => new OptionsViewModel
                {
                    Id = st.Id,
                    Text = st.CorrectAnswer,
                    IsCorrect = true
                })
                .ToList();

            options.AddRange(shortTextOptions);

            return options;
        }


        public async Task<Response> UpdateQuiz(QuizViewModel model, int id)
        {
            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz != null)
            {
                // Update quiz details
                quiz.Name = model.Name;
                quiz.Description = model.Description;
                quiz.TotalDegree = model.TotalDegree;
                quiz.PassingScore = model.PassingDegree;
                quiz.IsPrivate = model.IsPrivate;
                quiz.Duration = model.Duration;
                quiz.CreatedOn = model.CreatedOn;
                quiz.SessionID = model.SessionID;

                foreach (var question in quiz.Questions)
                {
                    var questionModel = model.Questions.FirstOrDefault(q => q.Id == question.Id);
                    if (questionModel != null)
                    {
                        question.Title = questionModel.QuestionText;
                        question.Hint = questionModel.Hint;
                        question.Points = (double)questionModel.Points;

                        if (question.QuestionType == QuestionType.MultipleChoice)
                        {
                            foreach (var option in question.Options)
                            {
                                var optionModel = questionModel.Options.FirstOrDefault(o => o.Id == option.Id);
                                if (optionModel != null)
                                {
                                    option.Option = optionModel.Text;
                                    option.IsCorrect = optionModel.IsCorrect ?? false;

                                    // Ensure only one option is marked as correct
                                    if (optionModel.Text == questionModel.CorrectAnswer)
                                    {
                                        option.IsCorrect = true;
                                    }
                                    else
                                    {
                                        option.IsCorrect = false;
                                    }
                                }
                            }
                        }
                        else if (question.QuestionType == QuestionType.TrueFalse || question.QuestionType == QuestionType.ShortText)
                        {
                            var correctAnswer = questionModel.CorrectAnswer;
                            if (!string.IsNullOrEmpty(correctAnswer))
                            {
                                var option = question.Options.FirstOrDefault();
                                if (option != null)
                                {
                                    option.Option = correctAnswer;
                                    option.IsCorrect = true;
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return new Response { IsDone = true, Model = quiz };
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
        public async Task<bool> IsOpened(string UserId, int QuizID)
        {
            if (!string.IsNullOrEmpty(UserId) && QuizID != 0)
            {
                var isOpened = await _context.OpenedQuizzes
                    .FirstOrDefaultAsync(q => q.QuizId == QuizID && q.UserId==UserId);
                if(isOpened != null)
                {
                    return isOpened.IsOpened ? true : false;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> IsQuizForStudentSections(string userId, string sessionID)
        {
            var sections =
                _context.Sections
                .Join(_context.StudentsSections,
                se => se.Id,
                ss => ss.SectionId,
                (se, ss) => new { se, ss })
                .Join(_context.Subjects,
                se => se.se.SubjectId,
                su => su.Id,
                (se, su) => new { se.se, se.ss, su })
                .Join(_context.Quiz,
                su => su.su.Id,
                q => q.SubjectId,
                (su, q) => new { su.se, su.su, q, su.ss })
                .Join(_context.Users,
                ss => ss.ss.UserId,
                u => u.Id,
                (ss, u) => new { ss.su, ss.q, ss.ss, u })
                .Where(q => q.q.SessionID == sessionID && q.u.Id == userId)
                .ToList();
            if (sections.Any())
            {
                return true;
            }
            return false;
        }
    }
}
