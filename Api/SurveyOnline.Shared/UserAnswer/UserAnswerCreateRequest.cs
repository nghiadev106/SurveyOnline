namespace SurveyOnline.Shared.UserAnswer
{
    public class UserAnswerCreateRequest
    {
        public int? AnswerId { get; set; }
        public int SurveyId { get; set; }
        public string UserId { get; set; }
        public int QuestionId { get; set; }
        public string Response { get; set; }
    }
}
