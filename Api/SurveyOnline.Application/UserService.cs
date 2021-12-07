using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    //public class UserService:IUserService
    //{
    //    private IUserRepository _userRepository;
    //    private IUnitOfWork _unitOfWork;
    //    private readonly string Secret;

    //    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork,
    //         IConfiguration configuration)
    //    {
    //        _userRepository = userRepository;
    //        _unitOfWork = unitOfWork;
    //        Secret = configuration["AppSettings:Secret"];
    //    }


    //    public async Task<User> Authenticate(AuthenticateUserRequest request)
    //    {
    //        var user =await _userRepository.Login(request);
    //        return user;
    //    }

    //    public async Task<UserDto> GenerateToken(User user)
    //    {
    //        var role = await _userRepository.GetRoleByUser(user.RoleId);

    //        // authentication successful so generate jwt token
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = Encoding.ASCII.GetBytes(Secret);
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(new Claim[]
    //            {
    //                new Claim(ClaimTypes.Name, user.FullName.ToString()),
    //                new Claim(ClaimTypes.Email, user.Email.ToString()),
    //                new Claim(ClaimTypes.Role, role.Name.ToString())
    //            }),
    //            Expires = DateTime.UtcNow.AddDays(7),
    //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //        };
    //        var token = tokenHandler.CreateToken(tokenDescriptor);
           
    //        return new UserDto
    //        {
    //            Email = user.Email,
    //            FullName = user.FullName,
    //            Role = role.Name,
    //            Token = tokenHandler.WriteToken(token)
    //        };
    //    }

    //    public async Task<int> Add(RegisterUserRequest request)
    //    {
    //        User user = new User();
    //        user.Id = Guid.NewGuid().ToString();
    //        user.Email = request.Email;
    //        user.FullName = request.FullName;
    //        user.RoleId = request.RoleId;
    //        user.Password = request.Password;

    //        await _userRepository.Add(user);
    //        var result = await _userRepository.Commit();
    //        return result;
    //    }

    //    public async Task<User> CheckEmail(string email)
    //    {
    //        var user = await _userRepository.CheckEmail(email);
    //        return user;
    //    }
    //}
}
