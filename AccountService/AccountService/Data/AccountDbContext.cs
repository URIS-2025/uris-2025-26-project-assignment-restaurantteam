using Microsoft.EntityFrameworkCore;
using AccountService.Entities;

namespace AccountService.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        //public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User-Address kao value object (owned type)
            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Address, a =>
                {
                    a.Property(p => p.Street).HasMaxLength(200);
                    a.Property(p => p.StreetNumber);
                    a.Property(p => p.PostalCode);
                    a.Property(p => p.Country).HasMaxLength(100);
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}