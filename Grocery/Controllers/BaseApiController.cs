using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Grocery.Controllers
{

   // [Route("api/[controller]")]
    [EnableRateLimiting("fixed")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
