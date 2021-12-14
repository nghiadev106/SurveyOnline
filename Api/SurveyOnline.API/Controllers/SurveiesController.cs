using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserAnswerService _userAnswerService;
        private readonly IMapper _mapper;

        public SurveiesController(
            ISurveyService surveyService,
            IQuestionService questionService,
            IUserAnswerService userAnswerService,
        IMapper mapper)
        {
            _surveyService = surveyService;
            _questionService = questionService;
            _userAnswerService = userAnswerService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetAll()
        {
            var lstSurveies = await _surveyService.GetAll();
            var lstSurveiesVm = _mapper.Map<IEnumerable<SurveyDto>>(lstSurveies);
            return Ok(lstSurveiesVm);
        }

        [HttpGet("paging")]
        [Authorize(Policy = "RequireLoggedIn")]
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
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> GetById(int surveyId)
        {          
            var survey = await _surveyService.GetById(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));
            var surveyVm = _mapper.Map<SurveyDto>(survey);
            return Ok(surveyVm);
        }


        [HttpGet("getDetail/{surveyId}/user/{userId}")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> GetSurveyDetail(string userId, int surveyId)
        {
            var userAnswer =await _userAnswerService.CheckUserAnswer(userId, surveyId);
            if (userAnswer != null)
            {
                return BadRequest(new ApiBadRequestResponse("Bạn đã làm khảo sát này!"));
            }
            var survey = await _surveyService.GetSurveyDetail(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));
            return Ok(survey);
        }

        [HttpGet("getUserStatistics/{surveyId}")]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> GetUserStatistics(int surveyId)
        {
            var count = await _surveyService.GetUserStatistics(surveyId);
            return Ok(count);
        }

        [HttpGet("getRatioStatistics/{surveyId}")]
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public IActionResult GetRatioStatistics(int surveyId)
        {
            var survey = _surveyService.GetRatioStatistics(surveyId);
            return Ok(survey);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminOrCustomer")]
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
        [Authorize(Policy = "RequireAdminOrCustomer")]
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
        [Authorize(Policy = "RequireAdminOrCustomer")]
        public async Task<IActionResult> Delete(int surveyId)
        {
            var survey = await _surveyService.GetById(surveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khảo sát Id: {surveyId}"));

            var answerAnswer = await _userAnswerService.GetUserAnswerBySurveyId(surveyId);

            if (answerAnswer != null)
            {
                await _userAnswerService.Delete(answerAnswer);
            }

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
