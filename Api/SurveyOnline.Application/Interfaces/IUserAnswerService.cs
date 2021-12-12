using SurveyOnline.EntityFrameworkCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface IUserAnswerService
    {
        Task<int> Add(List<UsersAnswer> request);

        Task<UsersAnswer> CheckUserAnswer(string userId, int surveyId);

        Task<UsersAnswer> CheckAnswer(string userId, int surveyId, int questionId, int? answerId);
    }
}
