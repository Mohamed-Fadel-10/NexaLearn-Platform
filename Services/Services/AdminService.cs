using Entities.Models;
using Infrastructure.Data;
using Infrastructure.Response;
using Infrastructure.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly QuizContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(QuizContext _context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }
      
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var students = await _context.Users.ToListAsync();
            if (students.Any())
            {
                return students;
            }
            return new List<ApplicationUser>();

        }
        public async Task<List<IdentityRole>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            if (roles.Any())
            {
                return roles;
            }
            return new List<IdentityRole>();

        }
        public async Task<Response> AddRole(AddRoleViewModel role)
        {
            if (await _roleManager.RoleExistsAsync(role.Name))
                return new Response { Message = "Role Already Created Before", IsDone = false };
            IdentityRole identityRole = new IdentityRole(role.Name);
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return new Response { Message = "Role Created Successfully", IsDone = true };
            }
            return new Response { Message = "Role Created Failed", IsDone = false };
        }

        public async Task<Response> AddRoleRoUser(AddUserToRoleViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserID);
            if (user != null)
            {
                if (await _userManager.IsInRoleAsync(user, model.RoleID))
                {
                    return new Response { IsDone = false, Message = "This User Already in This Role" };
                }
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == model.RoleID);
                if (role == null)
                {
                    return new Response { IsDone = false, Message = "This Role Not Found" };
                }
                await _userManager.AddToRoleAsync(user, role.Name);
                return new Response { IsDone = true, Message = "User Added To Role Successfully" };
            }
            return new Response { IsDone = false, Message = "User Not Found" };
        }
        public async Task<Response> AddUser(AddUserViewModel model)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
            };
            var isEmailExist = await _userManager.FindByEmailAsync(user.Email);
            if (isEmailExist != null)
            {
                return new Response { IsDone = false, Message = "This Email Is Used Before" };
            }
            var isUserNameExist = await _userManager.FindByNameAsync(user.UserName);
            if (isUserNameExist != null)
            {
                return new Response { IsDone = false, Message = "This UserName Is Used Before" };
            }
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return new Response { IsDone = true, Message = "User Add Successfully", Model = model };
            }
            return new Response { IsDone = false, Message = "Can not Add User Try Again", Model = null };
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
            return false;

        }
        public async Task<List<UsersEvaluationViewModel>> Filtrations(FilterUsersEvaluationViewModel model)
        {
            if (model != null)
            {
                var students = _context.UsersQuestionsQuizzes
                            .Join(_context.Quiz,
                                  uqq => uqq.QuizID,
                                  q => q.Id,
                                  (uqq, q) => new { uqq, q })
                            .Join(_context.Users,
                                  uq => uq.uqq.UserId,
                                  u => u.Id,
                                  (uq, u) => new { uq.uqq, uq.q, u })
                            .Join(_context.Subjects,
                                  uq => uq.q.SubjectId,
                                  su => su.Id,
                                  (uq, su) => new { uq.uqq, uq.q, su })
                            .Join(_context.Sections,
                            su => su.su.Id,
                            se => se.SubjectId,
                            (su, se) => new { su.su, su.q, su.uqq, se })
                            .Join(_context.StudentsSections,
                            se => se.se.Id,
                            ss => ss.SectionId,
                            (se, ss) => new { se.uqq, se.su,se.se, se.q, ss })
                            .Where(s => s.ss.UserId == s.uqq.UserId);

                if (model.QuizId.HasValue)
                {
                    students = students.Where(c => c.q.Id == model.QuizId);
                }
                if (model.SectionId.HasValue)
                {
                    students = students.Where(c => c.se.Id == model.SectionId);
                }
                if (model.SubjectId.HasValue)
                {
                    students = students.Where(c => c.su.Id == model.SubjectId);
                }
               
                var result= await students.ToListAsync();

                return result
                              .GroupBy(g => new { g.q.Id, g.q.SessionID, g.uqq.UserId, g.su.Name ,g.se})
                              .Select(s => new UsersEvaluationViewModel
                              {
                                  QuizID = s.Key.Id,
                                  QuizName=s.Key.Name,
                                  QuizSession = s.Key.SessionID,
                                  UserName = _context.Users.FirstOrDefault(u => u.Id == s.Key.UserId).UserName,
                                  Score = s.Sum(uqq => uqq.uqq.Score),
                                  SubmissionDate = s.Max(uqq => uqq.uqq.SubmissionDate),
                                  Subject = s.Key.Name,
                                  Section=s.Key.se.Name
                              })
                              .OrderBy(s=>s.SubmissionDate)
                              .ThenBy(s=>s.Score)
                              .ToList();
            }

            return new List<UsersEvaluationViewModel>();
        }


    }
}
