using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class MenuItemIngredient
    {
        [Key]
        public int IdMenuItem { get; set; }
        public MenuItem MenuItem { get; set; }
        public int IdIngredient { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}