using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.Answers
{
    public class AnswerCreateRequest
    {
        [Required(ErrorMessage = "Bạn phải chọn câu hỏi")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập nội dung")]
        public string Content { get; set; }
    }
}
