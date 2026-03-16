using Microsoft.EntityFrameworkCore;
using URIS.Entities;

namespace URIS.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
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

        }
    }
}
