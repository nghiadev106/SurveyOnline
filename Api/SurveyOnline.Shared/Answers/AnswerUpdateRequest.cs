using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Answers
{
    public class AnswerUpdateRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn câu hỏi")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập nội dung")]
        public string Content { get; set; }
    }
}
