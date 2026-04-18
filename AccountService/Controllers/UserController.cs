using AccountService.DTO.Address;
using AccountService.DTO.User;
using AccountService.Entities;
using AccountService.Handlers.Address;
using AccountService.Handlers.Links;
using AccountService.Handlers.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
            {
                return Conflict(ModelState);
            }
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
            if (!ModelState.IsValid)
            {
                return Conflict(ModelState);
            }
            UserDTO user;
            AddressDTO address;
            if (updateUserDTO.Address != null)
            {
                address = await addressHandler.CreateAddress(updateUserDTO.Address);
                user = await userHandler.UpdateUser(updateUserDTO, idUser, address.IdAddress);
                user.Address = address;

            }
            else
            {
                user = await userHandler.UpdateUser(updateUserDTO, idUser, null);

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
