using Entities.Models;
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

    }
}
