using Microsoft.AspNetCore.Mvc;

namespace Data.Api
{
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        public IActionResult GetData()
        {
            return Ok();
        }
    }
}
