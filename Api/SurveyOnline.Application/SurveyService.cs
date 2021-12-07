using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Infrastructure.Infrastructure;
using SurveyOnline.Infrastructure.Repositories.Interfaces;
using SurveyOnline.Shared.Surveies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.Application
{
    public class SurveyService : ISurveyService
    {
        private ISurveyRepository _surveyRepository;
        private IUnitOfWork _unitOfWork;

        public SurveyService(ISurveyRepository SurveyRepository, IUnitOfWork unitOfWork)
        {
            _surveyRepository = SurveyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(SurveyCreateRequest request)
        {
            Survey survey = new Survey();
            survey.Name = request.Name;
            survey.Description = request.Description;
            survey.CategoryId = request.CategoryId;
            survey.StartDate = request.StartDate;
            survey.EndDate = request.EndDate;
            survey.NumberOfQuestion = request.NumberOfQuestion;
            survey.Status = request.Status;
            survey.CreateDate = DateTime.Now;
            survey.CreateBy = request.CreateBy;

            await _surveyRepository.Add(survey);
            var result = await _surveyRepository.Commit();
            return result;
        }

        public async Task<int> Delete(Survey survey)
        {
            _surveyRepository.Delete(survey);
            var result = await _surveyRepository.Commit();
            return result;
        }

        public async Task<IEnumerable<Survey>> GetAll()
        {
            return await _surveyRepository.GetAll();
        }

        public async Task<IEnumerable<Survey>> GetAllPaging(int? categoryId, string keyword)
        {
            var query= await _surveyRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);
            return query;
        }

        public async Task<Survey> GetById(int id)
        {
            return await _surveyRepository.GetById(id);
        }

        public async Task<SurveyVm> GetSurveyDetail(int surveyId)
        {
            var survey = await _surveyRepository.GetSurveyDetail(surveyId);
            return survey;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> Update(Survey request)
        {
            await _surveyRepository.Update(request);
            var result = await _surveyRepository.Commit();
            return result;
        }
    }
}
