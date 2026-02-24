using Microsoft.EntityFrameworkCore;
using ReservationService.Entities;

namespace ReservationService.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Table)
                .WithMany()
                .HasForeignKey(r => r.IdTable)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}