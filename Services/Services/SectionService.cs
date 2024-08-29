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
    public class SectionService : GenericRepository<Section> ,ISectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SectionService(QuizContext context, IUnitOfWork _unitOfWork) :base(context) {
            this._unitOfWork = _unitOfWork;
        }


        public async Task<Response> AddSection(SectionViewModel model)
        {
            if (model != null)
            {
                var section = new Section();
                section.Name = model.Name;
                section.Place = model.Address;
                section.Code = Guid.NewGuid().ToString().Substring(1, 8);
                section.SubjectId = model.SubjectId;
                section.Capacity = model.Capacity;
                await _unitOfWork.Section.AddAsync(section);
                await _unitOfWork.SaveAsync();
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


        public async Task<IEnumerable<Section>> GetAllSections()
        {
            var sections = await _unitOfWork.Section.GetAllAsync();
           return sections.Any() ? sections : new List<Section>();

        }

        public async Task<IEnumerable<Section>> SectionsBySubjectID(int id)
        {
            var sections = await _unitOfWork.Section
                .Filter(s => s.SubjectId == id);
          return sections.Any()?sections : new List<Section>();

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

                return sections.Any()? sections : new List<Section>();
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
            var result =await _unitOfWork.Section.GetAllAsync();
            if (sectionId.HasValue)
            {
                result = result.Where(s=>s.Id==sectionId).ToList();
            }

            if (result.Any())
                return result
                    .Select(s => new SectionStudentsDataViewModel
                    {
                      SectionName=  s.Name,
                      StudentsNumber = _context.StudentsSections.Count(ss => ss.SectionId == s.Id),
                      SectionCode=s.Code
                    }).ToList();

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

