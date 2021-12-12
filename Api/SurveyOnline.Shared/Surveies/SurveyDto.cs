using System;

namespace SurveyOnline.Shared.Surveies
{
    public class SurveyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CategoryId { get; set; }
        public int? NumberOfQuestion { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public int? Status { get; set; }
    }
}
