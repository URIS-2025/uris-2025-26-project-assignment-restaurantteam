using Microsoft.EntityFrameworkCore;
using AuthenticationService.Entities;

namespace AuthenticationService.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
           : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresss { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.IdAddress)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

           base.OnModelCreating(modelBuilder);
            var address1 = modelBuilder.Entity<Address>().HasData([
                    new Address
                    {
                        idAddress = 100,
                        Street = "Pavla Papa",
                        City = "Novi Sad",
                        StreetNumber = 12,
                        Country = "Serbia",
                        PostalCode = 11220
                    }
             ]);
            
            _ = modelBuilder.Entity<User>().HasData([
                    new User
                    {
                        IdUser = 100,
                        Username = "ADMIN",
                        Email = "email@gmail.com",
                        PhoneNumber = "02345",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 12),
                        IdAddress = 100,
                        Role = Entities.Enums.UserRole.ADMIN
                    }
             ]);
        }
    }
}
