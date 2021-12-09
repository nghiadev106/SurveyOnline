using System;

namespace SurveyOnline.Shared.Questions
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SurveyId { get; set; }
        public int SortOrder { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
