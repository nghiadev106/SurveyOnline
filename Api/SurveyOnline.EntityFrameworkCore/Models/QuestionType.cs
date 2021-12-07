using System;
using System.Collections.Generic;

#nullable disable

namespace SurveyOnline.EntityFrameworkCore.Models
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
