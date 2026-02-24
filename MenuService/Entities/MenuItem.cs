using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class MenuItem
    {
        public int IdMenuItem { get; set; }
        public string MenuItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }

        public ICollection<MenuItemCategory> MenuItemCategories { get; set; }
        public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}