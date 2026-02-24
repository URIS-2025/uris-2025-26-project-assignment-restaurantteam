using MenuService.DTO.Category;
using MenuService.DTO.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.DTO.MenuItem
{
    public class MenuItemResponseDto
    {
        public int IdMenuItem { get; set; }
        public string MenuItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }

        // Kategorije za prikaz
        public List<CategoryDto> Categories { get; set; }

        // Sastojci za prikaz
        public List<IngredientDto> Ingredients { get; set; }
    }
}