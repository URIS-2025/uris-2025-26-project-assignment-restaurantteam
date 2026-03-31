using Microsoft.EntityFrameworkCore;
using ReservationService.Entities;

namespace ReservationService.Data
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
   : base(options) { }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
