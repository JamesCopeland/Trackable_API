using Microsoft.AspNetCore.Mvc;

namespace Trackable_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicController : Controller
    {
        public readonly ILogger<PublicController> _logger;

        public PublicController(ILogger<PublicController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok( new { date = DateTime.UtcNow.ToString() });
        }
    }
}
