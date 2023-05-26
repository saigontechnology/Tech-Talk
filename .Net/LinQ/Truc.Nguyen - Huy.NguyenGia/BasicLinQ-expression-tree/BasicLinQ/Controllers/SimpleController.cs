using Microsoft.AspNetCore.Mvc;

namespace BasicLinQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleController : Controller
    {
        [HttpGet]
        public IActionResult SimpleOfType()
        {
            return Ok();
        }
    }
}
