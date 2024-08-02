﻿using Entities.Models;
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
        public Task<List<Quiz>> GetAllQuizzes();
        public Task<Response> GetById(int id);
        public Task<Response> UpdateQuiz(QuizViewModel model, int id);
        public Task<Response> DeleteQuiz(int id);
        public Task<QuizViewModel> Details(int id);
        public Task<Response> MakePrivate(int id, bool isPrivate);
        public Task<Response> MakeEnabled(int id, bool IsEnabled);
        public Task<bool> IsOpened(string UserId, int QuizID);
    }
}
