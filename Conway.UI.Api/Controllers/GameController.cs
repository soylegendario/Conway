using Microsoft.AspNetCore.Mvc;

namespace Conway.UI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class GameController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("GameController is working!");
        }
    }
}
