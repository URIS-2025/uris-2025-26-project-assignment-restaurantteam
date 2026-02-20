using Microsoft.EntityFrameworkCore;
using ReservationService.Domain.Entities;

namespace ReservationService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.IdReservation);

                entity.Property(r => r.Status)
                      .HasConversion<string>();

                entity.HasOne(r => r.Table)
                      .WithMany()
                      .HasForeignKey(r => r.IdTable)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(t => t.IdTable);

                entity.Property(t => t.Status)
                      .HasConversion<string>();
            });
        }
    }
}