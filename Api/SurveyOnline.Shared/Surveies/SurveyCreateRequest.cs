using System;
using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Surveies
{
    public class SurveyCreateRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập tên khảo sát")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn danh mục khảo sát")]
        public int CategoryId { get; set; }

        [Range(1, 10, ErrorMessage = "Số câu hỏi từ 1 đến 10 câu")]
        //[MaxLength(2, ErrorMessage = "max lengh 2")]
        public int? NumberOfQuestion { get; set; }
        public string CreateBy { get; set; }
        public int? Status { get; set; }
    }
}
