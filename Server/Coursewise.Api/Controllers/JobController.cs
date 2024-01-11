using Coursewise.Api.Extensions;
using Coursewise.Api.Utility;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(
            IJobService jobService)
        {
            _jobService = jobService;            
        }
        [HttpGet("get/{id}")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetJobByCustomer(Guid id)
        {
            var customerId = HttpContext.GetCustomerId();
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var result = await _jobService.GetByCustomer(id,customerId,userId);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("all/by/customer")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetAllJobsByCustomer()
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var response = await _jobService.GetForCustomer(userId);
            return Ok(response);
        }
        [HttpGet("all")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> GetAllJobsByAdmin()
        {
            var customerId = HttpContext.GetCustomerId();
            var response = await _jobService.GetAll(customerId);
            return Ok(response);
        }
        [HttpPost("")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        [ValidateModelState]
        public async Task<IActionResult> Add([FromForm] Domain.Models.Dto.JobDto job)
        {
            var result = await _jobService.Add(job);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        [ValidateModelState]
        public async Task<IActionResult> Update([FromForm] Domain.Models.Dto.EditJobDto job)
        {
            var result = await _jobService.Update(job);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("upload/picture")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        [ValidateModelState]
        public async Task<IActionResult> UploadPicture([FromForm] Domain.Models.Dto.JobPictureUploadDto pictureModel)
        {
            var result = await _jobService.UploadPicture(pictureModel);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _jobService.Get(id);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _jobService.Delete(id);
            return result.success ? Ok(result) : BadRequest(result);

        }

        [HttpGet("Count")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetJobsCount()
        {
            var result = await _jobService.GetJobsCounts();

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("Get/Filters")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetJobsFilters()
        {

            var result = await _jobService.CalculateJobsCounts();

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("filters")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetJobsByFilters(Domain.Models.JobFilters filters)
        {
            var customerId = HttpContext.GetCustomerId();
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var result = await _jobService.Filter(filters,customerId,userId);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update/filters/count")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> UpdateJobCounts()
        {
            var result = await _jobService.UpdateJobsCounts();

            return result.success ? Ok(result) : BadRequest(result);
        }

    }
}
