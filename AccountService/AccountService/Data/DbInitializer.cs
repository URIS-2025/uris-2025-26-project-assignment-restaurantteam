using System.Linq;
using AccountService.Entities;
using AccountService.Entities.Enums;

namespace AccountService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AccountDbContext context)
        {
            context.Database.EnsureCreated();

            // Ako već postoje korisnici, ništa ne radimo
            if (context.Users.Any()) return;

            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@example.com",
                PhoneNumber = "123456789",
                Role = UserRole.ADMIN,
                Password = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 12)
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}