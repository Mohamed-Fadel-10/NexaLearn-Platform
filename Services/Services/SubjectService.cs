using Entities.Models;
using Infrastructure.Data;
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
    public class SubjectService: GenericRepository<Subject>, ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubjectService(QuizContext _context, IUnitOfWork _unitOfWork) : base(_context)
        {
           this._unitOfWork = _unitOfWork;
        }

        public async Task<Response> AddSubject(Subject model)
        {
            if (model != null)
            {
                await _unitOfWork.Subject.AddAsync(model);
                var row = await _unitOfWork.SaveAsync();
                return new Response { IsDone = true , Model=model };
            }
            return new Response { IsDone = false };
        }
        public async Task<IEnumerable<Subject>> GetAllSubjects()
        {
            var subjects = await _unitOfWork.Subject.GetAllAsync();
            return subjects.Any() ? subjects :new List<Subject>();
        }

    }
}
