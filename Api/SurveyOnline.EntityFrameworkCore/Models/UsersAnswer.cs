using System;
using System.Collections.Generic;

#nullable disable

namespace SurveyOnline.EntityFrameworkCore.Models
{
    public partial class UsersAnswer
    {
        public int Id { get; set; }
        public int? AnswerId { get; set; }
        public string UserId { get; set; }
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Response { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Question Question { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
