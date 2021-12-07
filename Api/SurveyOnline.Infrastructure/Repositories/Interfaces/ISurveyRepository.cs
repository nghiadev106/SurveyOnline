using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Shared.Surveies;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface ISurveyRepository : IRepository<Survey>
    {
        Task<SurveyVm> GetSurveyDetail(int surveyId);
    }
}
