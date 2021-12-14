using SurveyOnline.EntityFrameworkCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface IUserAnswerService
    {
        Task<int> Add(List<UsersAnswer> request);

        Task<int> Delete(List<UsersAnswer> usersAnswers);

        Task<UsersAnswer> CheckUserAnswer(string userId, int surveyId);

        Task<List<UsersAnswer>> GetUserAnswerBySurveyId(int surveyId);

        Task<UsersAnswer> CheckAnswer(string userId, int surveyId, int questionId, int? answerId);
    }
}
