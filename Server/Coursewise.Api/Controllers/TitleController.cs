using AutoMapper;
using Coursewise.Common.Models;
using Coursewise.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly ITitleService _titleService;
        private readonly IMapper _mapper;

        public TitleController(ITitleService titleService,
            IMapper mapper)
        {
            _titleService = titleService;
            _mapper = mapper;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _titleService.Get();
            if (response.Any())
            {
                return Ok(BaseModel.Success(_mapper.Map<List<Domain.Models.Dto.EntityDropdown>>(response)));
            }
            var noLocationText = "No title available";
            return Ok(BaseModel.Error(noLocationText));
        }
    }
}
