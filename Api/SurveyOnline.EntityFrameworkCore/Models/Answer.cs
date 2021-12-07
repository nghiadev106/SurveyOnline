using System;
using System.Collections.Generic;

#nullable disable

namespace SurveyOnline.EntityFrameworkCore.Models
{
    public partial class Answer
    {
        public Answer()
        {
            UsersAnswers = new HashSet<UsersAnswer>();
        }

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<UsersAnswer> UsersAnswers { get; set; }
    }
}
