using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class UserAnswerService: IUserAnswerService
    {
        private IUserAnswerRepository _userAnswerRepository;
        private IUnitOfWork _unitOfWork;

        public UserAnswerService(IUserAnswerRepository userAnswerRepository, IUnitOfWork unitOfWork)
        {
            _userAnswerRepository = userAnswerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(List<UsersAnswer> request)
        {
            await _userAnswerRepository.Add(request);
            var result = await _userAnswerRepository.Commit();
            return result;
        }

        public async Task<UsersAnswer> CheckUserAnswer(string userId, int surveyId)
        {
             var query = await _userAnswerRepository.CheckUserAnswer(userId, surveyId);
            return query;
        }

        public async Task<List<UsersAnswer>> GetUserAnswerBySurveyId(int surveyId)
        {
            List<UsersAnswer> query = await _userAnswerRepository.GetUserAnswerBySurveyId(surveyId);
            return query;
        }

        public async Task<UsersAnswer> CheckAnswer(string userId, int surveyId, int questionId, int? answerId)
        {
            var query = await _userAnswerRepository.CheckAnswer(userId, surveyId,questionId,answerId);
            return query;
        }

        public Task<int> Delete(List<UsersAnswer> usersAnswers)
        {
             _userAnswerRepository.Delete(usersAnswers);
            var result=  _userAnswerRepository.Commit();
            return result;
        }
    }
}
