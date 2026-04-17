using AccountService.DTO.User;
using AccountService.DTO.Address;
using AccountService.Handlers.Address;
using AccountService.Handlers.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AccountService.Handlers.Links;

namespace AccountService.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserHandler userHandler;
        private readonly IAddressHandler addressHandler;
        private readonly IOrderLink orderLink;
        private readonly IReservationLink reservationLink;



        public UserController(IUserHandler userHandler, 
                                IAddressHandler addressHandler,
                                IOrderLink orderLink,
                                IReservationLink reservationLink)
        {
            this.userHandler = userHandler;
            this.addressHandler = addressHandler;
            this.orderLink = orderLink;
            this.reservationLink = reservationLink;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            AddressDTO address = new AddressDTO();
            UserDTO user;
            if (createUserDTO.Address != null)
            {
                address = await addressHandler.CreateAddress(createUserDTO.Address);
                user = await userHandler.CreateUserWithAddress(createUserDTO, address.IdAddress);

                user.Address = address;
            }
            else
            {
                user = await userHandler.CreateUser(createUserDTO);
            }

                return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await userHandler.GetUsers();
            return Ok(users);
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] int idUser)
        {
            var user = await userHandler.GetUserById(idUser);
            return Ok(user);
        }


        [HttpPut("{idUser}")]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromRoute] int idUser, [FromBody] UpdateUserDTO updateUserDTO)
        {
            var user = await userHandler.UpdateUser(updateUserDTO,idUser);
            if(updateUserDTO.Address != null)
            {
                var address = await addressHandler.UpdateAddress(updateUserDTO.Address, user.Address.IdAddress);
                user.Address = address;
            }

            return Ok(user);
        }

        [HttpPatch("{idUser}")]
        public async Task<ActionResult<UserDTO>> UpdateUserRole([FromRoute] int idUser, [FromBody] UpdateUserRoleDTO updateUserRoleDTO)
        {
            var user = await userHandler.UpdateUserRole(updateUserRoleDTO, idUser);
            return Ok(user);
        }


        [HttpDelete("{idUser}")]
        public async Task<ActionResult<UserDTO>> DeleteUser([FromRoute] int idUser, [FromHeader] string? authorization)
        {
            var user = await userHandler.GetUserById(idUser);
            bool? isInUseInReservations = await reservationLink.IsUserInUse(idUser, authorization);
            bool? isInUseInOrders = await orderLink.IsUserInUse(idUser, authorization);

            if(isInUseInReservations.HasValue == true)
            {
                return Conflict("User has reservations.");
            }
            else if (isInUseInOrders.HasValue == true)
            {
                return Conflict("User has orders.");

            }

            var isDeleted = await userHandler.DeleteUser(idUser);
            return Ok(user);
        }
    }  
}
