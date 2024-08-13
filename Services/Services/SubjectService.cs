using Entities.Models;
using Infrastructure.Data;
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
    public class SubjectService: ISubjectService
    {
        private readonly QuizContext _context;
        public SubjectService(QuizContext context) {  _context = context; }

        public async Task<Response> AddSubject(SubjectViewModel model)
        {
            if (model != null)
            {
                var subject = new Subject();
                subject.Name = model.Name;
                subject.MaxDegree = model.MaxDegree;
                subject.MinDegree = model.MinDegree;
                await _context.Subjects.AddAsync(subject);
                await _context.SaveChangesAsync();
                return new Response
                {
                    IsDone = true,
                    Model = model
                };
            }
            return new Response
            {
                IsDone = false,
                Model = null
            };

        }

        public async Task<List<Subject>> GetAllSubjects()
        {
            var subjects = await _context.Subjects.Include(s => s.Sections).ToListAsync();
            if (subjects.Any())
            {
                return subjects;
            }
            return new List<Subject>();

        }

    }
}
