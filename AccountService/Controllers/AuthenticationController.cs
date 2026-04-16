using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AccountService.Handlers.Authentication;
using AccountService.Data;
using AccountService.DTO.Authentication;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationHandler authenticationHandler;
        private readonly AccountDbContext _context;


        public AuthenticationController(IAuthenticationHandler authenticationHandler, AccountDbContext context)
        {
            this.authenticationHandler = authenticationHandler;
            _context = context;
        }
      

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginRequest)
        {

            var token = await authenticationHandler.Login(loginRequest);
            return Ok(token);
        }
    
    }
}
