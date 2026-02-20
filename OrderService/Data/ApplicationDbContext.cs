using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.IdOrder);

                entity.OwnsOne(o => o.TotalPrice, money =>
                {
                    money.Property(m => m.Amount)
                         .HasColumnName("TotalAmount")
                         .HasPrecision(18, 2);

                    money.Property(m => m.Currency)
                         .HasColumnName("Currency")
                         .HasMaxLength(3);
                });

                entity.Property(o => o.OrderStatus)
                      .HasConversion<string>();

                entity.Property(o => o.PaymentMethod)
                      .HasConversion<string>();
            });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.IdOrder);
        }
    }
}