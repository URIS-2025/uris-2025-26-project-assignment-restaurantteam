using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItemCategory>()
                .HasKey(mc => new { mc.IdMenuItem, mc.IdCategory });

            modelBuilder.Entity<MenuItemIngredient>()
                .HasKey(mi => new { mi.IdMenuItem, mi.IdIngredient });
        }
    }
}