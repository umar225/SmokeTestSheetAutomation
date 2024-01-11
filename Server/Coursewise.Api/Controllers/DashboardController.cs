using Coursewise.Api.Extensions;
using Coursewise.Api.Utility;
using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Entities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IResourceService _resourceService;


        public DashboardController(
            IJobService jobService,
            IResourceService resourceService)
        {
            _jobService = jobService;
            _resourceService = resourceService;
        }
        [HttpGet("get")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetDashboardData()
        {
            BaseModel result = new BaseModel();
            DashboardDto dashboardDto = new DashboardDto();
            var customerId = HttpContext.GetCustomerId();
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;

            dashboardDto.Jobs= await _jobService.GetLatest(customerId, userId);
            dashboardDto.Resources = await _resourceService.GetLatest();

            result.success = true;
            result.data = dashboardDto;

            return result.success ? Ok(result) : BadRequest(result);
        }
        
    }
}
