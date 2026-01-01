using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIMonitizationGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        [HttpGet("RequestSend")]
        public IActionResult RequestSend()
        {
            return Ok("Request allowed");
        }
    }
}
