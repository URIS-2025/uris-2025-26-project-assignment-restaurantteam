using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.DTO.MenuItem
{
    public class UpdateMenuItemDto
    {
        public string MenuItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public bool IsAvailable { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> IngredientIds { get; set; }
    }
}