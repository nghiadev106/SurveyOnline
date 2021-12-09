﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurveyOnline.API.Helpers;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.EntityFrameworkCore.Models;
using SurveyOnline.Shared.UserAnswer;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswersController : ControllerBase
    {
        private readonly IUserAnswerService _userAnswerService;
        private readonly IMapper _mapper;

        public UserAnswersController(
            IUserAnswerService userAnswerService,
            IMapper mapper)
        {
            _userAnswerService = userAnswerService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<UserAnswerCreateRequest> request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse("Dữ liệu không hợp lệ"));
            }

            List<UsersAnswer> usersAnswers = new List<UsersAnswer>();
            foreach (var item in request)
            {
                UsersAnswer entity = new UsersAnswer();
                entity.AnswerId = item.AnswerId;
                entity.UserId = item.UserId;
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
                return BadRequest(new ApiBadRequestResponse("Thêm mới không thành công"));
            }
        }
    }
}
