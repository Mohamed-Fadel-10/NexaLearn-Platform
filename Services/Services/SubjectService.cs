using Entities.Models;
using Infrastructure.Data;
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
