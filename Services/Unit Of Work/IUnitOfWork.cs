using Entities.Interfaces;
using Entities.Models;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Unit_Of_Work
{
    public interface IUnitOfWork:IDisposable
    {
        public IGenericRepository<Subject> Subject { get; }
        public IGenericRepository<ApplicationUser> Students { get; }
        public IGenericRepository<Section> Section { get; }
        public IGenericRepository<StudentsSections> StudentSections { get;}
        public IGenericRepository<OpenedQuizzes> OpenedQuizzes { get; }
        public IGenericRepository<UsersQuestionsQuiz> UsersQuestionsQuiz { get; }
        public IGenericRepository<Quiz> Quiz { get; }
        public IGenericRepository<Materials> Materials { get; }




        public Task<int> SaveAsync();
    }
}
