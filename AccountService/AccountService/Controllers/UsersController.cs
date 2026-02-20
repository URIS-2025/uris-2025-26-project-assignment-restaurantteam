using AccountService.Data;
using AccountService.DTO.Users;
using AccountService.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AccountDbContext _context;

        public UsersController(AccountDbContext context)
        {
            _context = context;
        }

        [HttpPut("{idUser}/role")]
        public IActionResult ChangeRole(int idUser, ChangeRoleDto dto)
        {
            var user = _context.Users.Find(idUser);
            if (user == null)
                return NotFound();

            user.ChangeRole(Enum.Parse<UserRole>(dto.Role));
            _context.SaveChanges();

            return Ok("Role updated");
        }
    }
}