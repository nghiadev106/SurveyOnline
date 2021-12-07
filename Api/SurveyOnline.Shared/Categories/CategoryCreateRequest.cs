using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Categories
{
    public class CategoryCreateRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập tên danh mục")]
        [StringLength(250, ErrorMessage = "Tên danh mục không quá 250 ký tự")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
