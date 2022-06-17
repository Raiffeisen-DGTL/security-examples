using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAuth2._0
{
  [Route("[controller]")]
  public class HomeController : ControllerBase
  {
    [Authorize]
    [HttpGet("[action]")]
    public ActionResult Secret()
    {
      return Ok(User.Claims.Select(x => $"{x.Type} {x.Value}"));
    }
  }
}
