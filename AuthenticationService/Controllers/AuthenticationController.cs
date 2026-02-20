using AuthenticationService.Data.Auth;
using AuthenticationService.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationHelper _authenticationHelper;

        public AuthenticationController(IAuthenticationHelper authenticationHelper)
        {
            _authenticationHelper = authenticationHelper;
        }

        /// <summary>
        /// Sluzi za autentifikaciju korisnika
        /// </summary>
        /// <param name="principal">Model sa podacima na osnovu kojih se vrši autentifikacija</param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Authenticate(Principal principal)
        {
            //Pokušaj autentifikacije
            if (_authenticationHelper.AuthenticatePrincipal(principal))
            {
                var tokenString = _authenticationHelper.GenerateJwt(principal);
                return Ok(new { token = tokenString });
            }

            //Ukoliko autentifikacija nije uspela vraća se status 401
            return Unauthorized();
        }
    }
}