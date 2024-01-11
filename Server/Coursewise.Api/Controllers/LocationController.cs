using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Common.Utilities;
using Coursewise.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = CoursewiseRoles.ADMIN)]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public LocationController(ILocationService locationService,
            IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _locationService.Get();
            if (response.Any())
            {
                return Ok(BaseModel.Success(_mapper.Map< List<Domain.Models.Dto.EntityDropdown>>(response)));
            }
            var noLocationText = "No location available";
            return Ok(BaseModel.Error(noLocationText));
        }
    }
}
