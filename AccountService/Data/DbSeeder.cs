using AccountService.Entities;
using System.Data;

namespace AccountService.Data
{
    public class DbSeeder
    {
        public static async Task SeedAdminAsync(AccountDbContext context)
        {
            if (!context.Users.Any(u => u.Role == Entities.Enums.UserRole.ADMIN && u.Username =="admin"))
            {
                var admin = new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = Entities.Enums.UserRole.ADMIN
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
