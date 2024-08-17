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
    public class MaterialsService:GenericRepository<Materials> ,IMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaterialsService(QuizContext _context, IUnitOfWork _unitOfWork) :base(_context) {
            this._unitOfWork = _unitOfWork;
        }
        public async Task<Response> AddMaterials(AddMaterialsViewModel model)
        {
            if (model != null)
            {
                foreach (var item in model.Files)
                {
                    if (item.Length > 0)
                    {
                        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        Directory.CreateDirectory(uploadFolder);

                        var filePath = Path.Combine(uploadFolder, item.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }

                        var material = new Materials()
                        {
                            Title = model.Title,
                            Description = model.Description,
                            CreatedAt = DateTime.Now,
                            FilePath = filePath,  
                            SectionId = model.SectionId
                        };
                        await _unitOfWork.Materials.AddAsync(material);
                    }
                }
                await _unitOfWork.SaveAsync();
                return new Response { IsDone = true };
            }
            return new Response { IsDone = false };
        }

    }
}
