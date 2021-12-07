using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Surveies;
using System.Threading.Tasks;
using System.Linq;
using SurveyOnline.Shared.Questions;
using SurveyOnline.Shared.Answers;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class SurveyRepository : RepositoryBase<Survey>, ISurveyRepository
    {
        public SurveyRepository(IDbFactory dbFactory)
           : base(dbFactory) { }

        public async Task<SurveyVm> GetSurveyDetail(int surveyId)
        {
            SurveyVm query =await DbContext.Surveys.Where(x => x.Id == surveyId)
                                .Select( m => new SurveyVm
                                {
                                    Id=m.Id,
                                    Name = m.Name,
                                    Description = m.Description,
                                    StartDate = m.StartDate,
                                    EndDate = m.EndDate,
                                    CategoryName = m.Category.Name,                                  
                                    NumberOfQuestion = m.NumberOfQuestion,
                                    CreateDate = m.CreateDate,
                                    Questions = DbContext.Questions.Where(x => x.SurveyId == surveyId)
                                    .Select(q => new QuestionVm
                                    {
                                        Id=q.Id,
                                        Name=q.Name,
                                        Description=q.Description,
                                        SurveyId=m.Id,
                                        QuestionTypeId=q.QuestionTypeId,
                                        Answers= DbContext.Answers.Where(x => x.QuestionId == q.Id)
                                        .Select(a => new AnswerVm
                                        {
                                            Content=a.Content,
                                            QuestionId=q.Id,
                                            Id=a.Id
                                        }).ToList()
                                    }).ToList()
                                }).FirstOrDefaultAsync();

            return query;
        }
    }
}
