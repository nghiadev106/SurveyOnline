using SurveyOnline.Shared.Questions;
using System;
using System.Collections.Generic;

namespace SurveyOnline.Shared.Surveies
{
    public class SurveyVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CategoryName { get; set; }
        public int? NumberOfQuestion { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<QuestionVm> Questions { get; set; }
    }
}
