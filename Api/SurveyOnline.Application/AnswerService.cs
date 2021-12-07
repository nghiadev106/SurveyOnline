using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Answers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class AnswerService:IAnswerService
    {
        private IAnswerRepository _answerRepository;
        private IUnitOfWork _unitOfWork;

        public AnswerService(IAnswerRepository AnswerRepository, IUnitOfWork unitOfWork)
        {
            _answerRepository = AnswerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AnswerCreateRequest request)
        {
            Answer answer = new Answer();
            answer.QuestionId = request.QuestionId;
            answer.Content = request.Content;
            await _answerRepository.Add(answer);
            var result = await _answerRepository.Commit();
            return result;
        }

        public async Task<int> Delete(Answer answer)
        {
            _answerRepository.Delete(answer);
            var result = await _answerRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<Answer>> GetAll()
        {
            return await _answerRepository.GetAll();
        }

        public async Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId)
        {
            var answers = await _answerRepository.GetAnswerByQuestionId(questionId);
            return answers;
        }

        public async Task<Answer> GetById(int id)
        {
            return await _answerRepository.GetById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(Answer request)
        {
            await _answerRepository.Update(request);
            var result = await _answerRepository.Commit();
            return result;
        }
    }
}
