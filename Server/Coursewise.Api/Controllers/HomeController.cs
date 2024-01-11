using Microsoft.AspNetCore.Mvc;

namespace Coursewise.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly string _version;
        public HomeController(
            IConfiguration configuration)
        {
            _version = configuration["Nova:APIVersion"];
        }
        [HttpGet(Name = "Index")]
        public IActionResult Index()
        {
            ViewData.Add("Version", _version);
            return View();
        }
    }
}
