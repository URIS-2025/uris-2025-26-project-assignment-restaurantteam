using System.Threading.Tasks;
using AuthenticationService.Model;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data.User
{
    public class AuthUserRepository : IAuthUserRepository
    {
        private readonly AuthenticationDbContext _context;

        public AuthUserRepository(AuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task<AuthUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(AuthUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}