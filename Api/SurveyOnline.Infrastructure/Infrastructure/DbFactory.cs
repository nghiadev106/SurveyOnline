using Microsoft.EntityFrameworkCore;
using QuestionOnline.Infrastructure.Infrastructure;
using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        DbContextOptions<SurveyOnlineContext> _options;
        SurveyOnlineContext dbContext;

        public DbFactory(DbContextOptions<SurveyOnlineContext> options)
        {
            _options = options;
        }

        public SurveyOnlineContext Init()
        {
            return dbContext ?? (dbContext = new SurveyOnlineContext(_options));
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
