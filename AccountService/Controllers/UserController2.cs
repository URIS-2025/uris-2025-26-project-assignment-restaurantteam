using AccountService.DTO.User;
using AccountService.DTO.Address;
using AccountService.Handlers.Address;
using AccountService.Handlers.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AccountService.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController2 : ControllerBase
    {

        private readonly IUserHandler userHandler;
        private readonly IAddressHandler addressHandler;


        public UserController2(IUserHandler userHandler, IAddressHandler addressHandler)
        {
            this.userHandler = userHandler;
            this.addressHandler = addressHandler;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            var address = await addressHandler.CreateAddress(createUserDTO.Address);
            var user = await userHandler.CreateUser(createUserDTO, address.IdAddress);
            user.Address = address;
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
            var address = await addressHandler.UpdateAddress(updateUserDTO.Address, user.Address.IdAddress);
            user.Address = address;
            return Ok(user);
        }

        [HttpPatch("{idUser}")]
        public async Task<ActionResult<UserDTO>> UpdateUserRole([FromRoute] int idUser, [FromBody] UpdateUserRoleDTO updateUserRoleDTO)
        {
            var user = await userHandler.UpdateUserRole(updateUserRoleDTO, idUser);
            return Ok(user);
        }


        [HttpDelete("{idUser}")]
        public async Task<ActionResult<UserDTO>> DeleteUser([FromRoute] int idUser)
        {
            var user = await userHandler.DeleteUser(idUser);
            return Ok(user);
        }
    }  
}
