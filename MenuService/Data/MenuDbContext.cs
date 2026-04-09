using MenuService.Entities;
using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<MenuItemCategory>()
                .HasOne(u => u.Category)
                .WithMany(a => a.MenuItemCategories)
                .HasForeignKey(u => u.IdCategory)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuItemCategory>()
                .HasOne(u => u.MenuItem)
                .WithMany(a => a.MenuItemCategories)
                .HasForeignKey(u => u.IdMenuItem)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<MenuItemIngredient>()
                .HasOne(u => u.Ingredient)
                .WithMany(a => a.MenuItemIngredients)
                .HasForeignKey(u => u.IdIngredient)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuItemIngredient>()
                .HasOne(u => u.MenuItem)
                .WithMany(a => a.MenuItemIngredients)
                .HasForeignKey(u => u.IdMenuItem)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
