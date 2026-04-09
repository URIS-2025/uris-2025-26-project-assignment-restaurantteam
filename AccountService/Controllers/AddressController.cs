using AccountService.DTO.Address;
using AccountService.Handlers.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressHandler addressHandler;

        public AddressController(IAddressHandler addressHandler)
        {
            this.addressHandler = addressHandler;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<AddressDTO>> CreateAddress([FromBody] CreateAddressDTO createAddressDTO)
        {
            var address = await addressHandler.CreateAddress(createAddressDTO);
            return Ok(address);
        }

        [HttpGet]
        public async Task<ActionResult<List<AddressDTO>>> GetAddresses()
        {
            var addresses = await addressHandler.GetAddresses();
            return Ok(addresses);
        }

        [HttpGet("{idAddress}")]
        public async Task<ActionResult<AddressDTO>> GetAddressById( [FromRoute] int idAddress)
        {
            try
            {
                var address = await addressHandler.GetAddressById(idAddress);
                return Ok(address);
            }
            catch (Exception)
            {
                return NotFound("Address not found!");
            }

        }

        [HttpPut("{idAddress}")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress([FromBody] UpdateAddressDTO updateAddressDTO, [FromRoute] int idAddress)
        {
            int userId = int.Parse(User.FindFirst("id").Value);
            var role = User.FindFirst(ClaimTypes.Role).Value;

            if (role == null)
            {
                return Unauthorized();
            }

            try
            {
                var address = await addressHandler.UpdateAddress(updateAddressDTO, idAddress);
                return Ok(address);
            }
            catch (Exception)
            {
                return NotFound();
            }     
        }

        [HttpDelete("{idAddress}")]
        public async Task<ActionResult<bool>> DeleteAddress([FromRoute] int idAddress)
        {
            try
            {
                bool isDeleted = await addressHandler.DeleteAddress(idAddress);
                return Ok(isDeleted);
            }
            catch (Exception)
            {
                return NotFound();
            }

        }
    }
}
