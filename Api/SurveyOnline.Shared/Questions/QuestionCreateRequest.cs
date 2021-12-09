using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Questions
{
    public class QuestionCreateRequest
    {
        [Required(ErrorMessage = "Bạn phải nhập câu trả lời")]
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn khảo sát")]
        public int SurveyId { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn loại câu hỏi")]
        public int QuestionTypeId { get; set; }
    }
}
