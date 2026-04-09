using Microsoft.EntityFrameworkCore;
using AccountService.Entities;

namespace AccountService.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
           : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.IdAddress)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
