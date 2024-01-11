using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/health")]
    public class HealthController : Controller
    {
        [HttpGet(Name = "/health/index")]
        public IActionResult Index()
        {
            return Ok("Healthy");
        }
    }
}
