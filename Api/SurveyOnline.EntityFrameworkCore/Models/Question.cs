using System;
using System.Collections.Generic;

#nullable disable

namespace SurveyOnline.EntityFrameworkCore.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            UsersAnswers = new HashSet<UsersAnswer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SurveyId { get; set; }
        public int QuestionTypeId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual QuestionType QuestionType { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<UsersAnswer> UsersAnswers { get; set; }
    }
}
