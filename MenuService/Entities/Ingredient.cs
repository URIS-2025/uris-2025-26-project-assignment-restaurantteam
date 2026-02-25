using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class Ingredient
    {
        [Key]
        public int IdIngredient { get; set; }
        public string IngredientName { get; set; }
        public bool IsAllergen { get; set; }
        public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}