using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Shared.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<QuestionDto>> GetQuestionBySurveyId(int surveyId);

        Task<IEnumerable<QuestionType>> GetAllQuestionTypes();
    }
}
