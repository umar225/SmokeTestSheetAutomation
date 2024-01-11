using Coursewise.Common.Models;
using Coursewise.Domain.Interfaces;
using Coursewise.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService _levelService;
        
        public LevelController(ILevelService levelService)
        {
            _levelService=levelService;            
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllLevels()
        {
            var result = await _levelService.Get();
            return Ok(BaseModel.Success(result));
        }
    }
}
