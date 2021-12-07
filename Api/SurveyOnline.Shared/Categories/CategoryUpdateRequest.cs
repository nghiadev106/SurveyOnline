using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Shared.Categories
{
    public class CategoryUpdateRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tên danh mục")]
        [StringLength(250, ErrorMessage = "Tên danh mục không quá 250 ký tự")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
