using Microsoft.EntityFrameworkCore;
using MenuService.Entities;

namespace MenuService.Data
{
    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions<MenuDbContext> options)
            : base(options) { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<MenuItemIngredient> MenuItemIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItemCategory>()
                .HasKey(x => new { x.IdMenuItem, x.IdCategory });

            modelBuilder.Entity<MenuItemIngredient>()
                .HasKey(x => new { x.IdMenuItem, x.IdIngredient });

            base.OnModelCreating(modelBuilder);
        }
    }
}