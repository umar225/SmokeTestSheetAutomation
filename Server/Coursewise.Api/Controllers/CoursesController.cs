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
    public class CoursesController : ControllerBase
    {
        private readonly ICoursewiseLogger<CoursesController> _logger;
        private readonly ICourseService _coursesService;

        public CoursesController(ICoursewiseLogger<CoursesController> logger,
            ICourseService coursesService)
        {
            _logger = logger;
            _coursesService = coursesService;
        }
        [HttpPost("")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> Add(Course course)
        {
            var result = await _coursesService.Add(course);
            return result.success ? Ok(result) : BadRequest(result);

        }
        [HttpPut("")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> Edit(Course course)
        {
            var result = await _coursesService.Update(course);
            return result.success ? StatusCode(StatusCodes.Status201Created,result) : BadRequest(result);
            
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _coursesService.Delete(id);
            return result.success ? Ok(result) : BadRequest(result);

        }
        [HttpGet("coaching/{level}")]
        public async Task<IActionResult> CoachingCourses(string level)
        {
            _logger.Info($"Getting Coaching courses");
            var result = await _coursesService.GetCoachingCoursesByLevel(level);
            return result.success ? Ok(result) : NotFound();
        }

        [HttpGet("leadership/{level}")]
        public async Task<IActionResult> LeadershipCourses(string level)
        {
            _logger.Info($"Getting Leadership courses");
            var result = await _coursesService.GetLeadershipCoursesByLevel(level);
            return result.success ? Ok(result) : NotFound();
        }


        [HttpGet("pmp")]
        public async Task<IActionResult> PMPCourses()
        {
            _logger.Info($"Getting PMP courses");
            var courses = await _coursesService.GetPmpCourses();
            if (courses.Count==0)
            {
                return Ok(BaseModel.Error("No pmp courses exist"));
            }
            return Ok(BaseModel.Success(courses));
        }

        [HttpGet("library")]
        public async Task<IActionResult> GetAllCourses()
        {
            var coursesRespnose = await _coursesService.LibraryCoursesWithTypes();
            return coursesRespnose.success ? Ok(coursesRespnose) : NotFound(coursesRespnose);
        }

        [HttpGet("library/{name}")]
        public async Task<IActionResult> GetCourseByUrl(string name)
        {
            var coursesRespnose = await _coursesService.CourseByUrl(name);
            return coursesRespnose.success ? Ok(coursesRespnose) : NotFound(coursesRespnose);
        }

        [HttpGet("ned")]
        public async Task<IActionResult> NEDTraining()
        {
            _logger.Info($"Getting NED Training");
            var courses = await _coursesService.GetNedTraining();
            if (courses.Count == 0)
            {
                return Ok(BaseModel.Error("No ned training courses exist"));
            }
            return Ok(BaseModel.Success(courses));
        }

        [HttpGet("by/id/{id}")]
        public async Task<IActionResult> ById(Guid id)
        {
            var coursesRespnose = await _coursesService.CourseById(id);
            return Ok(coursesRespnose);
        }

        [HttpGet("all/details")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> GetAllCoursesByAdmin()
        {
            var coursesRespnose = await _coursesService.AllDetails();
            return Ok(coursesRespnose);
        }

        [HttpGet("all/by/category/{categoryId}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> GetAllCoursesByCategory(Guid categoryId)
        {
            var coursesRespnose = await _coursesService.CategoryWithCourses(categoryId);
            return Ok(coursesRespnose);
        }
    }
}