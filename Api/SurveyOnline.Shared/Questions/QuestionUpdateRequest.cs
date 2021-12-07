using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Questions
{
    public class QuestionUpdateRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập câu trả lời")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn khảo sát")]
        public int SurveyId { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn loại câu hỏi")]
        public int QuestionTypeId { get; set; }
    }
}
