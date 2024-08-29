using Entities.Interfaces;
using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Unit_Of_Work
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly QuizContext _context;
        public IGenericRepository<Subject> Subject { get; private set; }
        public IGenericRepository<ApplicationUser> Students { get; private set; }
        public IGenericRepository<Section> Section { get; private set; }
        public IGenericRepository<StudentsSections> StudentSections { get; private set; }
        public IGenericRepository<OpenedQuizzes> OpenedQuizzes { get; private set; }
        public IGenericRepository<UsersQuestionsQuiz> UsersQuestionsQuiz { get; private set; }
        public IGenericRepository<Quiz> Quiz { get; private set; }
        public IGenericRepository<Materials>Materials { get; private set; }
        public IGenericRepository<Question> Questions { get; private set; }




        public UnitOfWork (QuizContext context)
        {
            _context = context;
            Subject = new GenericRepository<Subject>(context);
            Students = new GenericRepository<ApplicationUser>(context);
            Section = new GenericRepository<Section>(context);
            StudentSections= new GenericRepository<StudentsSections>(context);
            OpenedQuizzes= new GenericRepository<OpenedQuizzes>(context);
            UsersQuestionsQuiz = new GenericRepository<UsersQuestionsQuiz>(context);
            Quiz= new GenericRepository<Quiz>(context);
            Materials= new GenericRepository<Materials>(context);
            Questions= new GenericRepository<Question>(context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
