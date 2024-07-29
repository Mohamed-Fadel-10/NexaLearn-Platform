using Entities.Models;
using Infrastructure.Data;
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
    public class SectionService : ISectionService
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
        public async Task<List<Section>> SectionsBySubjectID(int id)
        {
            var sections = await _context.Sections
                .Where(s => s.SubjectId == id)
                .ToListAsync();
            if (sections.Any())
            {
                return sections;
            }
            return new List<Section>();

        }
        public async Task<List<Section>> StudentSections(string userId)
        {
            var sections = _context.Sections
                .Join(_context.StudentsSections,
                s => s.Id,
                ss => ss.SectionId,
                (s, ss) => new { Section = s, StudentSections = ss })
                .Where(s => s.StudentSections.UserId == userId)
                .Select(s => s.Section)
                .ToList();
            if (sections.Any())
            {
                return sections;
            }
            return new List<Section>();

        }
        public async Task<List<SectionMaterialsViewModel>> GetSectionDetails(int sectionId)
        {
            var sectionDetails = await _context.Sections
                .Where(s => s.Id == sectionId)
                .Select(s => new SectionMaterialsViewModel
                {
                    SectionName = s.Name,
                    Materials = s.Materials.Select(m => new MaterialViewModel
                    {
                        Title = m.Title,
                        Description = m.Description,
                        CreatedAt = m.CreatedAt,
                        FilePath = m.FilePath // Ensure the full file path is saved in the database
                    }).ToList()
                })
                .ToListAsync();

            if (sectionDetails.Any())
            {
                return sectionDetails;
            }

            return new List<SectionMaterialsViewModel>();
        }

    }
}

