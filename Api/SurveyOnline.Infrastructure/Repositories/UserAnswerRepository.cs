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

        public async Task<UsersAnswer> GetSurveyDetail(string userId,int surveyId)
        {
            UsersAnswer query = await DbContext.UsersAnswers.Where(x => x.UserId == userId ).FirstOrDefaultAsync();

            return query;
        }
    }
}
