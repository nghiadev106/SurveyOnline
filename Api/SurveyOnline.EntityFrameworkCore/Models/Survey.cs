using System;
using System.Collections.Generic;

#nullable disable

namespace SurveyOnline.EntityFrameworkCore.Models
{
    public partial class Survey
    {
        public Survey()
        {
            Questions = new HashSet<Question>();
            UsersAnswers = new HashSet<UsersAnswer>();
        }

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

        public virtual Category Category { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UsersAnswer> UsersAnswers { get; set; }
    }
}
