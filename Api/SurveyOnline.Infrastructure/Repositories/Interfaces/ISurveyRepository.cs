using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Shared.Questions;
using SurveyOnline.Shared.Surveies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface ISurveyRepository : IRepository<Survey>
    {
        Task<SurveyVm> GetSurveyDetail(int surveyId);
        Task<int> GetUserStatistics(int surveyId);
        List<QuestionVm> GetRatioStatistics(int surveyId);
    }
}
