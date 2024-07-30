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
                        FilePath = m.FilePath 
                    }).ToList()
                })
                .ToListAsync();

            if (sectionDetails.Any())
            {
                return sectionDetails;
            }

            return new List<SectionMaterialsViewModel>();
        }
        public async Task<List<SectionStudentsDataViewModel>> SectionsWithStudentsNumbers(int? sectionId = null)
        {
            var result = await _context.Sections
                .Join(_context.StudentsSections,
                s => s.Id,
                ss => ss.SectionId,
                (s, ss) => new { Section = s, StudentSections = ss }).ToListAsync();

            if (sectionId.HasValue)
            {
                result = result.Where(s => s.Section.Id == sectionId).ToList();
            }

            if (result.Any())
                return result
                    .GroupBy(s => new { s.Section.Name ,s.Section.Code })
                    .Select(g => new SectionStudentsDataViewModel
                    {
                        StudentsNumber = g.Count(),
                        SectionName = g.Key.Name,
                        SectionCode=g.Key.Code
                    })
                    .ToList();

            return new List<SectionStudentsDataViewModel>();
        }

        public async Task<List<SectionStudentsDataViewModel>> StudentsInSection(int? sectionId=null)
        {
            var result =await  _context.Users
                .Join(_context.StudentsSections,
                u => u.Id,
                ss => ss.UserId,
                (u, ss) => new { Student = u, StudentSection = ss })
                .Join(_context.Sections,
                ss => ss.StudentSection.SectionId,
                s => s.Id,
                (ss, s) => new { StudentSection = ss, Section = s }
                ).AsNoTracking()
                .ToListAsync();
            if( sectionId.HasValue)
            {
                result = result.Where(s => s.Section.Id == sectionId).ToList();
            }
            if(result.Any())
            {
                return result.Select(s => new SectionStudentsDataViewModel
                {
                    StudentName=s.StudentSection.Student.Name,
                    SectionName=s.Section.Name
                }).ToList();
            }
            return new List<SectionStudentsDataViewModel>();
        }


    }
}

