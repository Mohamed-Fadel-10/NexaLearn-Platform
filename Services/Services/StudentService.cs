using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Enum;
using Infrastructure.Repsitories;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Unit_Of_Work;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class StudentService : GenericRepository<ApplicationUser>, IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(QuizContext context, IUnitOfWork _unitOfWork) :base(context)
        {
           this._unitOfWork = _unitOfWork;
        }

        public async Task<UsersEvaluationViewModel> Evaluate(List<UsersEvaluationViewModel> model)
        {
            if (model == null)
            {
                return new UsersEvaluationViewModel();
            }
            UsersEvaluationViewModel userdata = new UsersEvaluationViewModel();
            var questionIds = model.Select(x => x.QuestionID).ToList();

            userdata.QuestionsNumber = questionIds.Count;

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
                            if(item.Answer == correctAnswer)
                            {
                                item.Score = question.Points;
                                userdata.CorrectAnswerCount+=1;
                            }
                            else
                            item.Score = 0;
                        }
                    }
                    else if (question.QuestionType == QuestionType.TrueFalse)
                    {
                        if (trueFalseAnswers.TryGetValue(item.QuestionID, out var correctAnswer))
                        {
                            if (item.Answer == correctAnswer)
                            {
                                item.Score = question.Points;
                                userdata.CorrectAnswerCount+=1;
                            }
                            else
                                item.Score = 0;
                        }
                    }
                    else
                    {
                        if(ShortTextAnswers.TryGetValue(item.QuestionID,out var correctAnswer))
                        {
                            if (item.Answer == correctAnswer)
                            {
                                item.Score = question.Points;
                                userdata.CorrectAnswerCount+=1;
                            }
                            else
                                item.Score = 0;
                        }
                    }

                    usersQuestionsQuizzes.Add(
                        new UsersQuestionsQuiz
                    {
                        Answer = item.Answer,
                        UserId = item.UserId,
                        QuizID = item.QuizID,
                        QuestionID = item.QuestionID,
                        Score = item.Score,
                        SubmissionDate=DateTime.Now,                  
                    });
                }
                userdata.QuizID = item.QuizID;
                userdata.UserId = item.UserId;
            }

            // For Check in another Time if User Opened This Quiz Before we set Here Flag as True

            if (await _unitOfWork.OpenedQuizzes.FindFirst(s => s.UserId == userdata.UserId && s.QuizId == userdata.QuizID) == null) { 
            await _unitOfWork.OpenedQuizzes
                    .AddAsync(
                new OpenedQuizzes 
                { IsOpened = true, 
                  QuizId=userdata.QuizID,
                  UserId=userdata.UserId
                });
            }

            await _unitOfWork.UsersQuestionsQuiz.AddRangeAsync(usersQuestionsQuizzes);
            await _unitOfWork.SaveAsync();

            userdata.Score = usersQuestionsQuizzes
                .Where(u=>u.UserId==userdata.UserId&&u.QuizID==userdata.QuizID)
                .Select(u => u.Score)
                .Sum();

            var currentQuiz = await _unitOfWork.Quiz.FindFirst(q => q.Id == userdata.QuizID);

            var currentSubject= await _unitOfWork.Subject.FindFirst(s=>s.Id==currentQuiz.SubjectId);
            var currentSection = _context.Sections
                .Join(_context.StudentsSections, se => se.Id, ss => ss.SectionId, (se, ss) => new { Section = se, User = ss })
                .FirstOrDefault();


            return new UsersEvaluationViewModel()
            {
                UserId = userdata.UserId,
                UserName = await _context.Users.Where(u => u.Id == userdata.UserId).Select(u => u.UserName).FirstOrDefaultAsync(),
                QuizSession = currentQuiz.SessionID,
                QuizID=userdata.QuizID,
                Score = userdata.Score,
                QuizName= currentQuiz.Name,
                TotalDegree = currentQuiz.TotalDegree,
                QuestionsNumber=userdata.QuestionsNumber,
                CorrectAnswerCount=userdata.CorrectAnswerCount,
                Subject=currentSubject.Name,
                Section= currentSection.Section.Name,
            };
        }

        public async Task<Response> Enroll(string code,string UserID)
        {
            var section = await _unitOfWork.Section.FindFirst(c=>c.Code==code);
            if (section != null)
            {
                var isEnrolled = await _unitOfWork.StudentSections
                    .FindFirst(s => s.SectionId == section.Id && s.UserId == UserID);

                if (isEnrolled == null)
                {
                    await _unitOfWork.StudentSections
                        .AddAsync(
                        new StudentsSections 
                        {
                            SectionId = section.Id,
                            UserId = UserID
                        });

                    await _unitOfWork.SaveAsync();

                    return new Response { IsDone=true,Message="Enrolled Succeeded"};
                }
                return new Response { IsDone = false, Message = "You Already Enrolled In This Section Before" };
            }
            return new Response { IsDone = false, Message = "Section Not Found" };
        }
        public async Task<List<UserProfileDataViewModel>> GetAll()
        {
            var users = await _unitOfWork.Students.GetAllAsync();
            return users.Any() ? users.Select(s => new UserProfileDataViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                UserName = s.UserName,
                Phone = s.PhoneNumber,
                Major = s.Major
            })
                .ToList() :
                new List<UserProfileDataViewModel>();

        }

        public async Task<UserProfileDataViewModel> GetUser(string id)
        {
            var user = await _unitOfWork.Students.GetByIDAsync(id);
            return user != null ?
                new UserProfileDataViewModel
                {
                    Id=user.Id,
                    Name = user.Name,
                    UserName= user.UserName,
                    Phone=user.PhoneNumber,
                    Major= user.Major,
                    Email=user.Email
                } 
                : new UserProfileDataViewModel { };
        }

    
    }
}
