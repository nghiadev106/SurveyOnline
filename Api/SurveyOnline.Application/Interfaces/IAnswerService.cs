using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Shared.Answers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application.Interfaces
{
    public interface IAnswerService
    {
        Task<int> Add(AnswerCreateRequest request);

        Task<int> Update(Answer request);

        Task<int> Delete(Answer answer);

        Task<IEnumerable<Answer>> GetAll();

        Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId);

        Task<Answer> GetById(int id);

        void Save();
    }
}
