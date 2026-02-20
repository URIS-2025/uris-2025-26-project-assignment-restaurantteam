using System.Collections.Generic;

namespace AccountService.Models
{
    public class Ingredient
    {
        public int IdIngredient { get; set; }
        public string IngredientName { get; set; }
        public bool IsAllergen { get; set; }

        public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}
