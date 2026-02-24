using Microsoft.EntityFrameworkCore;
using AuthenticationService.Model;

namespace AuthenticationService.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthUser>().HasKey(u => u.IdUser);

            base.OnModelCreating(modelBuilder);
        }
    }
}