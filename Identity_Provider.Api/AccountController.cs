using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Provider.Api
{
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [Route("[action]")]
        public async Task<IActionResult> Login()
        {
            var request = Request;

            var authResult = await HttpContext.AuthenticateAsync(NegotiateDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
            {
                return Challenge(NegotiateDefaults.AuthenticationScheme);
            }

            var claims = authResult.Principal.Claims
                .Append(new Claim(JwtClaimTypes.Subject, authResult.Principal.FindFirstValue(ClaimTypes.Name)));

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    NegotiateDefaults.AuthenticationScheme
                    ));

            return SignIn(principal, IdentityServerConstants.DefaultCookieAuthenticationScheme);
        }


        [Authorize(AuthenticationSchemes = IdentityServerConstants.DefaultCookieAuthenticationScheme)]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

            return Ok();
        }
    }
}
