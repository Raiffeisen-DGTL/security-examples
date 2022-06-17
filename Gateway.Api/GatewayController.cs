using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api
{
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        [HttpGet("[action]")]
        [Authorize(Roles = "FieldOM_supportTeam")]
        [Authorize(Policy = "Policy Name")]
        public ActionResult Secret()
        {
            return Ok();
        }
    }
}
