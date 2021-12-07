using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Users
{
    public class RegisterUserRequest
    {
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Required(ErrorMessage = "Bạn phải nhập email")]
        [StringLength(256, ErrorMessage = "Email không quá 256 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập username")]
        [StringLength(256, ErrorMessage = "username không quá 256 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn quyền")]
        public int Role { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        [StringLength(50, ErrorMessage = "Mật khẩu không quá 50 ký tự")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không chính xác")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu xác nhận")]
        [StringLength(50, ErrorMessage = "Mật khẩu xác nhận không quá 50 ký tự")]
        public string ConfirmPassword { get; set; }
    }
}
