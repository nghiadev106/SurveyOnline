using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.Shared.UserAnswer
{
    public class UserAnswerCreateRequest
    {
        public int? AnswerId { get; set; }

        [Required]
        public int SurveyId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int QuestionId { get; set; }
        public string Response { get; set; }
    }
}
