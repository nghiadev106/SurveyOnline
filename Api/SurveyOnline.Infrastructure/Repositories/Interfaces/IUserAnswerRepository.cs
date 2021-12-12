using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface IUserAnswerRepository : IRepository<UsersAnswer>
    {
        Task<UsersAnswer> CheckUserAnswer(string userId, int surveyId);
        Task<UsersAnswer> CheckAnswer(string userId, int surveyId, int questionId, int? answerId);
    }
}
