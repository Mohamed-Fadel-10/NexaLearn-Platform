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
    public interface IStudentService
    {
        public Task<UsersEvaluationViewModel> Evaluate(List<UsersEvaluationViewModel> model);
        public Task<Section> Enroll(string code, string UserID);
        public Task<List<UserProfileDataViewModel>> GetAll();
    }
}
