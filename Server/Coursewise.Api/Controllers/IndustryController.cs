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
    public class IndustryController : ControllerBase
    {
        private readonly IIndustryService _industryService;
        private readonly IMapper _mapper;

        public IndustryController(IIndustryService industryService,
            IMapper mapper)
        {
            _industryService = industryService;
            _mapper = mapper;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _industryService.Get(); 
            if (response.Any())
            {
                return Ok(BaseModel.Success(_mapper.Map<List<Domain.Models.Dto.EntityDropdown>>(response)));
            }
            var noIndustryText = "No industries available";
            return Ok(BaseModel.Error(noIndustryText));
        }
    }
}
