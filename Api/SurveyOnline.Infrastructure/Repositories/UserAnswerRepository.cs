using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories
{
    public class UserAnswerRepository: RepositoryBase<UsersAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(IDbFactory dbFactory)
           : base(dbFactory) { }

        public async Task<UsersAnswer> CheckUserAnswer(string userId,int surveyId)
        {
            UsersAnswer query = await DbContext.UsersAnswers.Where(x => x.UserId == userId && x.SurveyId==surveyId).FirstOrDefaultAsync();
            return query;
        }

        public async Task<List<UsersAnswer>> GetUserAnswerBySurveyId(int surveyId)
        {
            List<UsersAnswer> query = await DbContext.UsersAnswers.Where(x => x.SurveyId == surveyId).ToListAsync();
            return query;
        }

        public async Task<UsersAnswer> CheckAnswer(string userId, int surveyId,int questionId,int? answerId)
        {
            if (answerId != null)
            {
                UsersAnswer query = await DbContext.UsersAnswers.Where(x => x.UserId == userId && x.SurveyId == surveyId && x.QuestionId == questionId && x.AnswerId == answerId).FirstOrDefaultAsync();
                return query;
            }
            else
            {
                UsersAnswer query = await DbContext.UsersAnswers.Where(x => x.UserId == userId && x.SurveyId == surveyId && x.QuestionId == questionId).FirstOrDefaultAsync();
                return query;
            }
        }
    }
}
