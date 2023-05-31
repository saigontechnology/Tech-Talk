using Microsoft.AspNetCore.Mvc;

namespace SampleWebApiAspNetCore.Controllers.v3
{
    [ApiController]
    [ApiVersion("3.1-RF")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FoodsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("3.1");
        }
    }
}
