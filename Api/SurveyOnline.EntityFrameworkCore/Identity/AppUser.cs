using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.EntityFrameworkCore.Identity
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
