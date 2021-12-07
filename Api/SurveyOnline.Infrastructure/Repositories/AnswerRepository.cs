using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(IDbFactory dbFactory)
          : base(dbFactory) { }

        public async Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId)
        {
            var answers = await DbContext.Answers.Where(x => x.QuestionId == questionId).ToListAsync();
            return answers;
        }
    }
}
