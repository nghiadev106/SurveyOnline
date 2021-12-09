using SurveyOnline.EntityFrameworkCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface IUserAnswerService
    {
        Task<int> Add(List<UsersAnswer> request);
    }
}
