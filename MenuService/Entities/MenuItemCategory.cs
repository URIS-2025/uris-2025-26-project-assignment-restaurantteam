using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenuService.Entities
{
    public class MenuItemCategory
    {
        [Key]
        public int IdMenuItem { get; set; }
        public MenuItem MenuItem { get; set; }
        public int IdCategory { get; set; }
        public Category Category { get; set; }
    }
}