using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Oms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderpoolController : ControllerBase
    {
        public IActionResult Index()
        {
            
            return Ok();
        }
    }
}