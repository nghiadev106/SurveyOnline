using Microsoft.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Users;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Infrastructure.Repositories
{
    //public class UserRepository : RepositoryBase<User>, IUserRepository
    //{
    //    public UserRepository(IDbFactory dbFactory)
    //       : base(dbFactory) { }

    //    public async Task<User> CheckEmail(string email)
    //    {
    //        var user = await DbContext.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
    //        return user;
    //    }

    //    public async Task<Role> GetRoleByUser(string roleId)
    //    {
    //        var role = await DbContext.Roles.Where(x => x.Id == roleId).SingleOrDefaultAsync();
    //        return role;
    //    }

    //    public async Task<User> Login(AuthenticateUserRequest request)
    //    {
    //        var userLogin = await DbContext.Users.Where(x => x.Email == request.Email && x.Password == request.Password).SingleOrDefaultAsync();
    //        return userLogin;
    //    }
    //}
}
