using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class QuestionService : IQuestionService
    {
        private IQuestionRepository _questionRepository;
        private IUnitOfWork _unitOfWork;

        public QuestionService(IQuestionRepository QuestionRepository, IUnitOfWork unitOfWork)
        {
            _questionRepository = QuestionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(QuestionCreateRequest request)
        {
            Question question = new Question();
            question.Name = request.Name;
            question.Description = request.Description;
            question.QuestionTypeId = request.QuestionTypeId;
            question.SurveyId = request.SurveyId;
            question.CreateDate = DateTime.Now;
            await _questionRepository.Add(question);
            var result = await _questionRepository.Commit();
            return result;
        }

        public async Task<int> Delete(Question question)
        {
            _questionRepository.Delete(question);
            var result = await _questionRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<Question>> GetAll()
        {
            return await _questionRepository.GetAll();
        }

        public async Task<Question> GetById(int id)
        {
            return await _questionRepository.GetById(id);
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionBySurveyId(int surveyId)
        {
            var questions = await _questionRepository.GetQuestionBySurveyId(surveyId);
            return questions;
        }

        public async Task<IEnumerable<QuestionType>> GetAllQuestionTypes()
        {
            var questionTypes = await _questionRepository.GetAllQuestionTypes();
            return questionTypes;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(Question request)
        {
            await _questionRepository.Update(request);
            var result = await _questionRepository.Commit();
            return result;
        }
    }
}
