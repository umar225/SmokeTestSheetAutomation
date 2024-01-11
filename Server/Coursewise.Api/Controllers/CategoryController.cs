using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = CoursewiseRoles.ADMIN)]
    public class CategoryController : ControllerBase
    {
        private readonly ICoursewiseLogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ICoursewiseLogger<CategoryController> logger,
            ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCoursesByAdmin()
        {
            var coursesRespnose = await  _categoryService.GetAll();
            if (coursesRespnose.Any())
            {
                _logger.Info($"{coursesRespnose.Count} categories have been returned.");
                return Ok(BaseModel.Success(coursesRespnose));
            }
            var noCategoryText = "No categories available";
            _logger.Info(noCategoryText);
            return Ok(BaseModel.Error(noCategoryText));
        }
        [HttpPut("")]
        public async Task<IActionResult> Edit(Category category)
        {
            var result = await _categoryService.Update(category);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("")]
        public async Task<IActionResult> Add(Category category)
        {
            var result = await _categoryService.Add(category);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle(Category category)
        {
            var result = await _categoryService.Toggle(category);

            return result.success ? Ok(result) : BadRequest(result);
        }
    }
}
