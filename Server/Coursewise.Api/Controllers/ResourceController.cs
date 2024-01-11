using Coursewise.Api.Extensions;
using Coursewise.Api.Utility;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Interfaces;
using Coursewise.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourceController(
            IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        [HttpPost("")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        [ValidateModelStateAttribute]
        public async Task<IActionResult> AddResource([FromForm] Domain.Models.Dto.AddResourceDto model)
        {
            var result = await _resourceService.Add(model);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        [ValidateModelStateAttribute]
        public async Task<IActionResult> UpdateResource(Domain.Models.Dto.EditResourceDto model)
        {
            var result = await _resourceService.Update(model);
            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var result = await _resourceService.Get(id);
            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("all")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _resourceService.GetAll();
            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("upload/media")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        [ValidateModelState]
        public async Task<IActionResult> UploadMedia([FromForm] Domain.Models.Dto.ResourceMediaUploadDto media)
        {
            var result = await _resourceService.UploadMedia(media);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("delete/media/{resourceId}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> DeleteMedia(int resourceId)
        {
            var result = await _resourceService.DeleteMedia(resourceId);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("delete/{resourceId}")]
        [Authorize(Policy = CoursewiseRoles.ADMIN)]
        public async Task<IActionResult> DeleteResource(Guid resourceId)
        {
            var result = await _resourceService.DeleteResource(resourceId);

            return result.success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("filters")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetJobsByFilters([FromQuery] Domain.Models.ResourceFilters filters)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var result = await _resourceService.GetByFilters(filters,userId);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("get/{name}")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetByUrl(string name)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;

            var result = await _resourceService.GetByUrl(name, userId);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("get/featured")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> GetFeatured()
        {
            var result = await _resourceService.GetFeatured();

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("applause/{id}")]
        [Authorize(Policy = CoursewiseRoles.CUSTOMER)]
        public async Task<IActionResult> Applause(Guid id)
        {
            var customerId = HttpContext.GetCustomerId();
            var userId = User.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var result = await _resourceService.Applause(customerId, id, userId);
            return result.success ? Ok(result) : BadRequest(result);
        }

    }
}
