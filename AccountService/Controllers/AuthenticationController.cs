using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AccountService.Handlers.Authentication;
using AccountService.Data;
using AccountService.DTO.Authentication;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationHandler authenticationHandler;
        private readonly AccountDbContext _context;


        public AuthenticationController(IAuthenticationHandler authenticationHandler, AccountDbContext context)
        {
            this.authenticationHandler = authenticationHandler;
            _context = context;
        }


        /*[HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {

            var existsAddress = await _context.Addresss.FirstOrDefaultAsync(u =>
                    u.City == dto.City &&
                    u.Country == dto.Country &&
                    u.Street == dto.Street &&
                    u.StreetNumber == dto.StreetNumber
                );
            var passedAddress = existsAddress;

            var address = new Address
            {
                City = dto.City,
                Country = dto.Country,
                Street = dto.Street,
                StreetNumber = dto.StreetNumber,
                PostalCode = dto.PostalCode,
            };

            if (existsAddress == null)
            {

                _context.Addresss.Add(address);

                await _context.SaveChangesAsync();
                passedAddress = address;
            }



            var exists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            if (exists) return Conflict("Korisnik već postoji");
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = _authHelper.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                IdAddress = passedAddress.idAddress
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Registracija uspješna");
        }*/

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginRequest)
        {
            Console.WriteLine("THese are the credentials " + loginRequest.Username + " and " + loginRequest.Password);

            var token = await authenticationHandler.Login(loginRequest);
         
            return Ok(token);
        }
    
    }
}
