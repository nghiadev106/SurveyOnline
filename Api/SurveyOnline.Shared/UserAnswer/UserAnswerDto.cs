using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Shared.UserAnswer
{
    public class UserAnswerDto
    {
        public int SurveyId { get; set; }
        public List<UserAnswerCreateRequest> Answers { get; set; }
    }
}
