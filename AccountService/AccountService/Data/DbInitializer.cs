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
                Password = "admin123" // kasnije hashirati
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
        }
    }
}