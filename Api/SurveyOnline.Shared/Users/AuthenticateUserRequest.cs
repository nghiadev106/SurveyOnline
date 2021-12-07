using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Users
{
    public class AuthenticateUserRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập Username")]
        [StringLength(50, ErrorMessage = "Username không quá 50 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [StringLength(50, ErrorMessage = "Mật khẩu không quá 250 ký tự")]
        public string Password { get; set; }
    }
}
