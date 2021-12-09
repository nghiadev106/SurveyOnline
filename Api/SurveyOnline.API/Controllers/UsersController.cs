using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyOnline.API.Email;
using SurveyOnline.API.Helpers;
using SurveyOnline.EntityFrameworkCore.Identity;
using SurveyOnline.Shared.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signManager;

        private readonly AppSettings _appSettings;

        private IEmailSender _emailsender;
        private ISendMailService _sendMailservice;

        public UsersController(UserManager<AppUser> userManager,
           SignInManager<AppUser> signInManager,
           IOptions<AppSettings> appSettings,
           IEmailSender emailsender,
           ISendMailService sendMailservice
           )
        {
            _userManager = userManager;
            _signManager = signInManager;
            _appSettings = appSettings.Value;
            _emailsender = emailsender;
            _sendMailservice = sendMailservice;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState,"Đăng ký không thành công"));
            }

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FullName= request.FullName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, request.Role == 1 ? "Customer":"Guest");

                // Sending Confirmation Email

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmEmail", "Users", new { UserId = user.Id, Code = code }, protocol: HttpContext.Request.Scheme);

                //await _emailsender.SendEmailAsync(user.Email, "Confirm Your Email", "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>");
                MailContent content = new MailContent
                {
                    To = user.Email,
                    Subject = "Confirm Your Email",
                    Body = "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>"
                };

                await _sendMailservice.SendMail(content);
                return Ok(new { username = user.UserName, FullName=user.FullName,email = user.Email, status = 1, message = "Đăng ký tài khoản thành công, vui lòng xác nhận Email" });

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(new ApiBadRequestResponse(ModelState, "Đăng ký không thành công"));

        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserRequest request)
        {
            // Get the User from Database
            var user = await _userManager.FindByNameAsync(request.Username);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.Secret));

            double tokenExpiryTime = Convert.ToDouble(_appSettings.ExpireTime);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {

                // THen Check If Email Is confirmed
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản chưa xác thực");
                    return Unauthorized(new ApiUnauthorizedResponse(ModelState, "Đăng nhập không thành công"));
                }

                // get user Role
                var roles = await _userManager.GetRolesAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim(ClaimTypes.Name,user.FullName),
                        new Claim("LoggedOn", DateTime.Now.ToString()),
                     }),

                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Site,
                    Audience = _appSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
                };

                // Generate Token

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new {
                    token = tokenHandler.WriteToken(token),
                    Expiration = token.ValidTo,
                    Username = user.UserName,
                    Email=user.Email,
                    FullName=user.FullName,
                    UserRole = roles.FirstOrDefault() 
                });

            }

            // return error
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác");
            return Unauthorized(new ApiUnauthorizedResponse(ModelState, "Đăng nhập không thành công"));

        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(new ApiBadRequestResponse(ModelState, "Xác nhận không thành công"));

            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError("", $"Không tìm thấy tài khoản Id: {userId}");
                return BadRequest(new ApiBadRequestResponse(ModelState, "Xác nhận không thành công"));
            }

            if (user.EmailConfirmed)
            {
                return Redirect(_appSettings.ReturnUrl);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Redirect(_appSettings.ReturnUrl);
            }
            else
            {
                List<string> errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.ToString());
                }
                return new JsonResult(errors);
            }
        }

        //public UsersController(
        //    IUserService userService,
        //    IMapper mapper)
        //{
        //    _userService = userService;
        //    _mapper = mapper;
        //}

        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest request)
        //{
        //    var user =await _userService.Authenticate(request);
        //    if (user == null)
        //        return BadRequest(new ApiBadRequestResponse("Email hoặc mật khẩu không chính xác"));

        //    // authentication successful so generate jwt token
        //    var userToken = await _userService.GenerateToken(user);
        //    return Ok(userToken);
        //}

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ApiBadRequestResponse("Bạn nhập không đúng định dạng"));
        //    }
        //    var user = await _userService.CheckEmail(request.Email);
        //    if (user != null)
        //        return BadRequest(new ApiBadRequestResponse("Email đã tồn tại trong hệ thống"));

        //    var result = await _userService.Add(request);
        //    if (result > 0)
        //    {
        //        return Ok(request);
        //    }
        //    else
        //    {
        //        return BadRequest(new ApiBadRequestResponse("Đăng ký tài khoản không thành công"));
        //    }
        //}
    }
}
