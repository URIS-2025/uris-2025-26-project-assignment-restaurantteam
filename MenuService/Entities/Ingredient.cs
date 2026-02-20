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

        private Ingredient() { }

        public Ingredient(string name, bool isAllergen)
        {
            IngredientName = name;
            IsAllergen = isAllergen;
        }
    }
}