using Entities.Models;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAdminService
    {
        public Task<Response> AddSubject(SubjectViewModel model);
        public Task<Response> AddSection(SectionViewModel model);
        public Task<List<Subject>> GetAllSubjects();
        public Task<List<ApplicationUser>> GetAllUsers();
        public Task<List<IdentityRole>> GetAllRoles();
        public Task<Response> AddRole(AddRoleViewModel role);
        public Task<Response> AddRoleRoUser(AddUserToRoleViewModel model);
        public Task<Response> AddUser(AddUserViewModel model);
        public Task<Response> DeleteUser(string id);
    }
}
