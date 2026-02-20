using Microsoft.EntityFrameworkCore;
using AccountService.Models;

namespace AccountService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MenuItem ↔ Category M:N
            modelBuilder.Entity<MenuItemCategory>()
                .HasKey(mc => new { mc.IdMenuItem, mc.IdCategory });

            modelBuilder.Entity<MenuItemCategory>()
                .HasOne(mc => mc.MenuItem)
                .WithMany(m => m.MenuItemCategories)
                .HasForeignKey(mc => mc.IdMenuItem);

            modelBuilder.Entity<MenuItemCategory>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MenuItemCategories)
                .HasForeignKey(mc => mc.IdCategory);

            // MenuItem ↔ Ingredient M:N
            modelBuilder.Entity<MenuItemIngredient>()
                .HasKey(mi => new { mi.IdMenuItem, mi.IdIngredient });

            modelBuilder.Entity<MenuItemIngredient>()
                .HasOne(mi => mi.MenuItem)
                .WithMany(m => m.MenuItemIngredients)
                .HasForeignKey(mi => mi.IdMenuItem);

            modelBuilder.Entity<MenuItemIngredient>()
                .HasOne(mi => mi.Ingredient)
                .WithMany(i => i.MenuItemIngredients)
                .HasForeignKey(mi => mi.IdIngredient);
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<MenuItemIngredient> MenuItemIngredients { get; set; }

    }
}
