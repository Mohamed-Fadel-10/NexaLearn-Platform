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
    public class SectionService:ISectionService
    {
        private readonly QuizContext _context;
        public SectionService(QuizContext context) { _context = context; }
        public async Task<List<Section>> GetAllSections()
        {
            var sections = await _context.Sections.ToListAsync();
            if (sections.Any())
            {
                return sections;
            }
            return new List<Section>();

        }
    }
}
