using Entities.Models;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISectionService
    {
        public Task<List<Section>> GetAllSections();
        public Task<List<Section>> SectionsBySubjectID(int id);
        public Task<List<Section>> StudentSections(string userId);
        public Task<List<SectionMaterialsViewModel>> GetSectionDetails(int sectionId);
        public Task<Response> AddSection(SectionViewModel model);
        public Task<List<SectionStudentsDataViewModel>> SectionsWithStudentsNumbers(int? sectionId=null);
        public Task<List<SectionStudentsDataViewModel>> StudentsInSection(int? sectionId = null);
    }
}
