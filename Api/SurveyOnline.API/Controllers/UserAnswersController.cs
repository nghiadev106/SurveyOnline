using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyOnline.API.Helpers;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Shared.UserAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswersController : ControllerBase
    {
        private readonly IUserAnswerService _userAnswerService;
        private readonly IQuestionService _questionService;

        public UserAnswersController(
            IUserAnswerService userAnswerService,
            IQuestionService questionService)
        {
            _userAnswerService = userAnswerService;
            _questionService = questionService;
        }

        [HttpPost]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> Create([FromBody] UserAnswerDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse("Dữ liệu không hợp lệ"));
            }

            var questions = await _questionService.GetQuestionBySurveyId(request.SurveyId);
            foreach(var item in questions)
            {
                if (request.Answers.Where(x => x.QuestionId == item.Id).Count() <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Bạn chưa trả lời câu {item.SortOrder}: {item.Name}");
                    return BadRequest(new ApiBadRequestResponse(ModelState,"Lưu khảo sát không thành công"));
                }
            }

            List<UsersAnswer> usersAnswers = new List<UsersAnswer>();
            foreach (var item in request.Answers)
            {
                if (item.AnswerId == null && item.Response==null)
                {
                    ModelState.AddModelError(string.Empty, $"Bạn chưa trả lời câu hỏi với Id:{item.QuestionId}");
                    return BadRequest(new ApiBadRequestResponse(ModelState, "Lưu khảo sát không thành công"));
                }
                var userAnswer =await _userAnswerService.CheckAnswer(item.UserId, item.SurveyId,item.QuestionId,item.AnswerId);
                if (userAnswer!=null)
                {
                    return BadRequest(new ApiBadRequestResponse("Bạn đã làm khảo sát này!"));
                }

                UsersAnswer entity = new UsersAnswer();
                entity.AnswerId = item.AnswerId;
                entity.UserId = item.UserId;
                entity.SurveyId = item.SurveyId;
                entity.QuestionId = item.QuestionId;
                entity.Response = item.Response;
                entity.CreateDate = DateTime.Now;

                usersAnswers.Add(entity);
            }

            var result = await _userAnswerService.Add(usersAnswers);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Lưu khảo sát không thành công"));
            }
        }
    }
}
