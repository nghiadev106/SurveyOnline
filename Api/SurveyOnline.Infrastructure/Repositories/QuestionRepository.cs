using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Questions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(IDbFactory dbFactory)
           : base(dbFactory) { }

        public async Task<IEnumerable<QuestionDto>> GetQuestionBySurveyId(int surveyId)
        {
            var questions = await DbContext.Questions.Where(x => x.SurveyId == surveyId)
                .Select(q => new QuestionDto { 
                    Id=q.Id,
                    Name=q.Name,
                    Description=q.Description,
                    SurveyId=q.SurveyId,
                    QuestionTypeName=q.QuestionType.Name,
                    CreateDate=q.CreateDate
                }).ToListAsync();            
            return questions;
        }

        public async Task<IEnumerable<QuestionType>> GetAllQuestionTypes()
        {
            var questionTypes = await DbContext.QuestionTypes.ToListAsync();
            return questionTypes;
        }
    }
}
