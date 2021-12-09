using System;
using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Surveies
{
    public class SurveyUpdateRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập tên khảo sát")]
        [StringLength(250, ErrorMessage = "Tên khảo sát không quá 250 ký tự")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn danh mục khảo sát")]
        public int CategoryId { get; set; }

        [Range(1, 10, ErrorMessage = "Số câu hỏi từ 1 đến 10 câu")]
        public int? NumberOfQuestion { get; set; }
        public int? Status { get; set; }
    }
}
