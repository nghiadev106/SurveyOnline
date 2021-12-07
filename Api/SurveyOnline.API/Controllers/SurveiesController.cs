using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurveyOnline.API.Helpers;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.Shared.Surveies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveiesController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public SurveiesController(
            ISurveyService surveyService,
            IQuestionService questionService,
            IMapper mapper)
        {
            _surveyService = surveyService;
            _questionService = questionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lstSurveies = await _surveyService.GetAll();
            var lstSurveiesVm = _mapper.Map<IEnumerable<SurveyDto>>(lstSurveies);
            return Ok(lstSurveiesVm);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging(int? categoryId, string keyword, int page, int pageSize = 10)
        {
            var lstSurveies = await _surveyService.GetAllPaging(categoryId, keyword);
            var lstSurveyVm = _mapper.Map<IEnumerable<SurveyDto>>(lstSurveies);
            var responseData = lstSurveyVm.OrderByDescending(x => x.CreateDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
                {
                Items = responseData,
                Page = page,
                TotalItems = lstSurveyVm.Count(),
                PageSize = pageSize
            });
        }

        [HttpGet("{surveyId}")]
        public async Task<IActionResult> GetById(int surveyId)
        {
            var survey = await _surveyService.GetById(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));
            var surveyVm = _mapper.Map<SurveyDto>(survey);
            return Ok(surveyVm);
        }

        [HttpGet("getDetail/{surveyId}")]
        public async Task<IActionResult> GetSurveyDetail(int surveyId)
        {
            var survey = await _surveyService.GetSurveyDetail(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));
            return Ok(survey);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SurveyCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Thêm mới không thành công"));
            }

            var result = await _surveyService.Add(request);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Thêm mới không thành công"));
            }
        }

        [HttpPut("{surveyId}")]
        public async Task<IActionResult> Update([FromRoute] int surveyId, [FromBody] SurveyUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Cập nhật không thành công"));
            }
            var survey = await _surveyService.GetById(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));

            var questions =await _questionService.GetQuestionBySurveyId(surveyId);
            if (questions.Count() > request.NumberOfQuestion)
            {
                return BadRequest(new ApiBadRequestResponse($"Số câu hỏi không được nhỏ hơn: {questions.Count()}"));
            }

            survey.Name = request.Name;
            survey.Description = request.Description;
            survey.CategoryId = request.CategoryId;
            survey.StartDate = request.StartDate;
            survey.EndDate = request.EndDate;
            survey.NumberOfQuestion = request.NumberOfQuestion;
            survey.Status = request.Status;
           
            var result = await _surveyService.Update(survey);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Cập nhật không thành công"));
            }
        }

        [HttpDelete("{surveyId}")]
        public async Task<IActionResult> Delete(int surveyId)
        {
            var survey = await _surveyService.GetById(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));

            var result = await _surveyService.Delete(survey);

            if (result > 0)
            {
                return Ok(survey);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Xóa khảo sát không thành công"));
            }
        }
    }
}
