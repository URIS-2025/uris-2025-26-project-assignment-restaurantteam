using System.Collections.Generic;

namespace AccountService.Models
{
    public class MenuItem
    {
        public int IdMenuItem { get; set; }
        public string MenuItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }

        // Veze M:N
        public ICollection<MenuItemCategory> MenuItemCategories { get; set; }
        public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}
