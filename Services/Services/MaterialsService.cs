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
    public class MaterialsService:IMaterialsService
    {
        private readonly QuizContext _context;
        public MaterialsService(QuizContext _context) {
           this._context= _context;
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
                        await _context.Materials.AddAsync(material);
                    }
                }
                await _context.SaveChangesAsync();
                return new Response { IsDone = true };
            }
            return new Response { IsDone = false };
        }

    }
}
