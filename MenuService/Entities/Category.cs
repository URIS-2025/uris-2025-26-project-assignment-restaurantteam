using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public ICollection<MenuItemCategory> MenuItemCategories { get; set; }
    }
}