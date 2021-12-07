using SurveyOnline.Shared.Answers;
using System.Collections.Generic;

namespace SurveyOnline.Shared.Questions
{
    public class QuestionVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public List<AnswerVm> Answers { get; set; }
    }
}
