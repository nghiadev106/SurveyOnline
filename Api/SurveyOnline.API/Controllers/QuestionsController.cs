using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurveyOnline.API.Helpers;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.Shared.Questions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly ISurveyService _surveyService;
        private readonly IMapper _mapper;

        public QuestionsController(
            IQuestionService questionService,
            ISurveyService surveyService,
            IMapper mapper)
        {
            _questionService = questionService;
            _surveyService = surveyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questions = await _questionService.GetAll();
            var questionsVm = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            return Ok(questionsVm);
        }

        [HttpGet("survey/{surveyId}")]
        public async Task<IActionResult> GetQuestionBySurveyId(int surveyId)
        {
            var questions = await _questionService.GetQuestionBySurveyId(surveyId);
            return Ok(questions);
        }

        [HttpGet("questionTypes")]
        public async Task<IActionResult> GetAllQuestionTypes()
        {
            var questionTypes = await _questionService.GetAllQuestionTypes();
            return Ok(questionTypes);
        }

        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var question = await _questionService.GetById(questionId);
            if (question == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found question with id {questionId}"));
            var questionVm = _mapper.Map<QuestionDto>(question);
            return Ok(questionVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState,"Thêm mới không thành công"));
            }
            var questions = await _questionService.GetQuestionBySurveyId(request.SurveyId);
            var survey = await _surveyService.GetById(request.SurveyId);
            if (survey == null)
                return NotFound(new ApiNotFoundResponse($"không tìm thấy khảo sát Id: {survey.Id}"));
            if (questions?.Count() >= 10)
            {
                return BadRequest(new ApiBadRequestResponse("Số câu hỏi không vượt quá 10"));
            }
            if (questions?.Count() >= survey?.NumberOfQuestion)
            {
                return BadRequest(new ApiBadRequestResponse($"Số câu hỏi không vượt quá {survey.NumberOfQuestion}"));
            }
            var result = await _questionService.Add(request);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Thêm câu hỏi không thành công"));
            }
        }

        [HttpPut("{questionId}")]
        public async Task<IActionResult> Update([FromRoute] int questionId, [FromBody] QuestionUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Cập nhật không thành công"));
            }

            var question = await _questionService.GetById(questionId);
            if (question == null)
                return NotFound(new ApiNotFoundResponse($"không tìm thấy câu hỏi Id: {questionId}"));

            question.Name = request.Name;
            question.Description = request.Description;
            question.QuestionTypeId = request.QuestionTypeId;
            var result = await _questionService.Update(question);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Cập nhật không thành công"));
            }
        }

        [HttpDelete("{questionId}")]
        public async Task<IActionResult> Delete(int questionId)
        {
            var question = await _questionService.GetById(questionId);
            if (question == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy câu hỏi Id: {questionId}"));

            var result = await _questionService.Delete(question);

            if (result > 0)
            {
                return Ok(question);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Xóa câu hỏi không thành công"));
            }
        }
    }
}
