using Infrastructure.Response;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
      public interface  IMaterialsService
      {
        public Task<Response> AddMaterials(AddMaterialsViewModel model);

      }
}
