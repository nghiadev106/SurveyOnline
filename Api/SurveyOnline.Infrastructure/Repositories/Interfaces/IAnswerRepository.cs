using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId);
    }
}
