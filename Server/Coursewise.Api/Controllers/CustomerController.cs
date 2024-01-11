using Coursewise.Api.Extensions;
using Coursewise.Api.Utility;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Interfaces;
using Coursewise.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IApplyJobService _applyJobService;
        private readonly IJobEmailService _jobEmailService;
        private readonly ICustomerService _customerService;

        public CustomerController(
            IApplyJobService applyJobService,
            IJobEmailService jobEmailService,
            ICustomerService customerService)
        {
            _applyJobService = applyJobService;
            _jobEmailService = jobEmailService;
            _customerService = customerService;
        }
        [HttpGet("basicInfo")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetBasicInfo()
        {
            var customerId = HttpContext.GetCustomerId();
            var result = await _customerService.GetBasicInfo(customerId);
            return result.success ? Ok(result) : BadRequest(result);

        }
        [HttpPost("oneToOne")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> GetOnetoOneUsers([FromBody] OneToOneFilters filters)
        {
            var result = await _customerService.GetOneToOneUsers(filters);
            return result.success ? Ok(result) : BadRequest(result);

        }
        [HttpPost("subscribeOneToOne")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> subscribeOneToOne([FromBody] SubscribeOneToOne subscribe)
        {
            var result = await _customerService.SubscribeOneToOne(subscribe);
            return result.success ? Ok(result) : BadRequest(result);

        }
        [HttpPost("/customer/updateNotifications")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        [ValidateModelStateAttribute]
        public async Task<IActionResult> UpdateNotifications([FromForm] CustomerNotifications model)
        {
            if (model != null)
            {
                var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;

                var result = await _customerService.UpdateNotifications(userId, model);
                return result.success ? Ok(result) : BadRequest(result);

            }
            return BadRequest();
        }
        [HttpPost("apply/job")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        [ValidateModelStateAttribute]
        public async Task<IActionResult> ApplyJob([FromForm] Domain.Models.Dto.ApplyJobDto model)
        {
            var token = HttpContext.GetCustomerWordpressToken();
            var customerId = HttpContext.GetCustomerId();
            var result = await _applyJobService.Apply(model,token,customerId);
            return result.success ? Ok(result) : BadRequest(result);
        }
       
        [HttpPut("email/about/job/{jobId}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> ApplyJob(Guid jobId)
        {
            var result = await _jobEmailService.SendEmailToCustomers(jobId);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("customers/import")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportCustomers()
        {
            var result =await _customerService.GetUsersfromCSV();
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("/customers/claims")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyClaims()
        {
            var result = await _customerService.VerifyUserClaims();
            return result.success ? Ok(result) : BadRequest(result);
        }
    }
}
