using SurveyOnline.EntityFrameworkCore.Models;
using System;

namespace SurveyOnline.Infrastructure.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        SurveyOnlineContext Init();
    }
}
