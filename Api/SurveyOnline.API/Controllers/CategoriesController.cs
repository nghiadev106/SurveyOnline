using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyOnline.API.Helpers;
using SurveyOnline.Application.Interfaces;
using SurveyOnline.Shared.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyOnline.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAll();
            var categoriesVm = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoriesVm);
        }


        [HttpGet("paging")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetAllPaging(string keyword, int page, int pageSize = 20)
        {
            var categories = await _categoryService.GetAllPaging(keyword);
            var categoriesVm = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            var responseData = categoriesVm.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new
            {
                Items = responseData,
                Page = page,
                TotalItems = categoriesVm.Count(),
                PageSize = pageSize
            });
        }

        [HttpGet("{categoryId}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {categoryId}"));
            var categoryVm = _mapper.Map<CategoryDto>(category);
            return Ok(categoryVm);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse("Dữ liệu không hợp lệ"));
            }
            var result = await _categoryService.Add(request);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Thêm mới không thành công"));
            }
        }

        [HttpPut("{categoryId}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiBadRequestResponse(ModelState, "Dữ liệu không hợp lệ"));
            }

            var category = await _categoryService.GetById(categoryId);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {categoryId}"));

            category.Name = request.Name;
            category.Description = request.Description;
            var result = await _categoryService.Update(category);
            if (result > 0)
            {
                return Ok(request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Cập nhật không thành công"));
            }
        }

        [HttpDelete("{categoryId}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            if (category == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với Id: {categoryId}"));

            var result =  await _categoryService.Delete(category);

            if (result > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Xóa không thành công"));
            }
        }
    }
}
