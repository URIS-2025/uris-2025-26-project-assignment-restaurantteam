using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class Ingredient
    {
        public int IdIngredient { get; private set; }
        public string IngredientName { get; private set; }
        public bool IsAllergen { get; private set; }
        public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}