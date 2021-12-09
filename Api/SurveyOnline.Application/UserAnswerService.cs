using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.UserAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
