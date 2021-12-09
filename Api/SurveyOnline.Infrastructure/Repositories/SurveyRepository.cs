using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Surveies;
using System.Threading.Tasks;
using System.Linq;
using SurveyOnline.Shared.Questions;
using SurveyOnline.Shared.Answers;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using System.Collections.Generic;
using System;
using System.Data;
using SurveyOnline.Infrastructure.Helpers;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class SurveyRepository : RepositoryBase<Survey>, ISurveyRepository
    {
        private IDatabaseHelper _dbHelper;
        public SurveyRepository(IDbFactory dbFactory, IDatabaseHelper dbHelper)
           : base(dbFactory)
        {
            _dbHelper = dbHelper;
        }

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
                                    Questions = DbContext.Questions.Where(x => x.SurveyId == surveyId).OrderBy(x=>x.SortOrder)
                                    .Select(q => new QuestionVm
                                    {
                                        Id=q.Id,
                                        Name=q.Name,
                                        Description=q.Description,
                                        SurveyId=m.Id,
                                        SortOrder=q.SortOrder,
                                        QuestionTypeId=q.QuestionTypeId,
                                        Answers_json = DbContext.Answers.Where(x => x.QuestionId == q.Id)
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

        public async Task<int> GetUserStatistics(int surveyId)
        {
            var query = from us in DbContext.UsersAnswers
                        join u in DbContext.AspNetUsers on us.UserId equals u.Id
                        join s in DbContext.Surveys on us.SurveyId equals s.Id
                        where s.Id == surveyId
                        select new{ UserId=u.Id };
            var result = query.GroupBy(x => x.UserId);
            return await result.CountAsync();       
        }

        public List<QuestionVm> GetRatioStatistics(int surveyId)
        {
            string msgError = "";
            try
            {
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "GetRatioStatistics",
                     "@surveyId", surveyId);
                if (!string.IsNullOrEmpty(msgError))
                    throw new Exception(msgError);
                return dt.ConvertTo<QuestionVm>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
