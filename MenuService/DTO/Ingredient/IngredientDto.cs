using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.DTO.Ingredient
{
    public class IngredientDto
    {
        public int IdIngredient { get; set; }
        public string IngredientName { get; set; }
        public bool IsAllergen { get; set; }
    }
}